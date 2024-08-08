using Godot;
using SrpgFramework.Global;
using SrpgFramework.Units;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SrpgFramework.CellGrid.Cells
{
	public partial class Cell : Node2D
	{
        public const string ResourcePath = "res://Resources/Prefabs/cell.tscn";
        /// <summary>
        /// 坐标
        /// </summary>
        public Vector2I Coord { get; internal set; }

        public GroundType GroundType { get; set; } = GroundType.Ground;

		private Unit _unit;
		public Unit Unit
		{
			get { return _unit; }
			set { _unit = value; }
		}

        /// <summary>
        /// 格子事件，用于处理例如陷阱等情况
        /// </summary>
        private ICellTrigger _cellTrigger;
        public ICellTrigger CellTriger
        {
            get { return _cellTrigger; }
            set
            {
                _cellTrigger?.Unregister(this);
                _cellTrigger = value;
                _cellTrigger?.Register(this);
            }
        }
        public int MoveCost { get; private set; } = 1;

        private HashSet<Cell> _neighbors;
        /// <summary>
        /// 周边格子 用于寻路计算
        /// </summary>
        public HashSet<Cell> Neighbors
        {
            get
            {
                if (_neighbors is null)
                {
                    _neighbors = BattleManager.CellGridMgr.Cells.Values.Where(c => GetDistance(c) == 1).ToHashSet();
                }
                return _neighbors;
            }
        }
        /// <summary>
        /// 计算A*寻路时的移动开销 默认为1
        /// </summary>
        public int MovenmentCost = 1;

        /// <summary>
        /// 该格子是否有Unit
        /// </summary>
        public bool IsTaken => Unit is not null;

        public override void _Ready()
        {
            var area = this.GetNode<Area2D>("Area2D");
            area.MouseEntered += MouseEnter;
            area.InputEvent += MouseDown;
        }

        /// <summary>
        /// 计算坐标间的曼哈顿距离
        /// </summary>
        public int GetDistance(Vector2I other)
        {
            var vec = Coord - other;
            return Mathf.Abs(vec.X) + Mathf.Abs(vec.Y);
        }
        /// <summary>
        /// 与其它格子的曼哈顿距离
        /// </summary>
        public int GetDistance(Cell cell)
        {
            return GetDistance(cell.Coord);
        }

        /// <summary>
        /// 以该格子为中心or起点，获取周边格子
        /// </summary>
        /// <param name="range">获取半径</param>
        /// <param name="includingSelf">是否包含自己</param>
        /// <param name="areaShape">形状</param>
        /// <returns></returns>
        public virtual HashSet<Cell> GetNeighborCells(int range, AreaShape areaShape = AreaShape.Circle, bool includingSelf = false)
        {
            HashSet<Cell> result = new();
            Action<Vector2I, HashSet<Cell>> tryAdd = (vec, result) =>
            {
                if (BattleManager.CellGridMgr.Cells.ContainsKey(vec))
                {
                    result.Add(BattleManager.CellGridMgr.Cells[vec]);
                }
            };

            switch (areaShape)
            {
                case AreaShape.Circle:
                    for (var i = 0; i <= range; i++)
                    {
                        for (var j = 0; j <= range - i; j++)
                        {
                            tryAdd(Coord + new Vector2I(i, j), result);
                            tryAdd(Coord + new Vector2I(-i, j), result);
                            tryAdd(Coord + new Vector2I(i, -j), result);
                            tryAdd(Coord + new Vector2I(-i, -j), result);
                        }
                    }
                    break;
                case AreaShape.Square:
                    for (var i = 0; i <= range; i++)
                    {
                        for (var j = 0; j <= range; j++)
                        {
                            tryAdd(Coord + new Vector2I(i, j), result);
                            tryAdd(Coord + new Vector2I(-i, j), result);
                            tryAdd(Coord + new Vector2I(i, -j), result);
                            tryAdd(Coord + new Vector2I(-i, -j), result);
                        }
                    }
                    break;
                case AreaShape.Cross:
                    result.Add(this);
                    for (var i = 1; i <= range; i++)
                    {
                        tryAdd(this.Coord + new Vector2I(i, 0), result);
                        tryAdd(this.Coord + new Vector2I(0, i), result);
                        tryAdd(this.Coord - new Vector2I(-i, 0), result);
                        tryAdd(this.Coord - new Vector2I(0, i), result);
                    }
                    break;
            }
            if (!includingSelf)
            {
                result.Remove(this);
            }
            return result;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            BattleManager.CellGridMgr.UnRegisterCell(this);
        }
    }

    public enum GroundType
    {
        Ground = 0b_0001,
        Water = 0b_0010,
        Unreachable = 0b_0000
    }

    public enum AreaShape
    {
        Circle,
        Square,
        Cross
    }
}