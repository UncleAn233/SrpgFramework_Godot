using Godot;
using SrpgFramework.CellGrid.CellGridState;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Level;
using SrpgFramework.Units;
using System.Collections.Generic;

namespace SrpgFramework.CellGrid
{
	public partial class CellGridManager : Node
    {
        public Dictionary<Vector2I, Cell> Cells { get; private set; } = new();

        public ICellGridState CurrentGridState { get; private set; }

        /// <summary>
        /// Default State As Block Input
        /// </summary>
        public ICellGridState BlockInputState { get; private set; }
        /// <summary>
        /// Default State As Idle
        /// </summary>
        public ICellGridState IdleState { get; private set; }
        /// <summary>
        /// Default State While an Unit is Selected
        /// </summary>
        public ICellGridState SelectUnitState { get; private set; }

        /// <summary>
        /// 当前选中单位
        /// </summary>
        private Unit _currentUnit;

        private Cell _highlightCell;
        
        /// <summary>
        /// 存储的tile map节点
        /// </summary>
        private TileMap _tileMap;

        public override void _Ready()
		{
            BlockInputState = new GridStateBlockInput();
            IdleState = new GridStateIdle(this);
            SelectUnitState = new GridStateSelectUnit(this);
            CurrentGridState = IdleState;

            BattleManager.LevelMgr.OnLevelLoad += GenerateCellGrid;
        }

        public bool ToState(ICellGridState nextState, Unit unit)
        {
            if (CurrentGridState.CanTranslateTo(nextState))
            {
                CurrentGridState?.Exit(_currentUnit);
                CurrentGridState = nextState;
                _currentUnit = unit;
                CurrentGridState.Enter(_currentUnit);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ToBlockInputState()
        {
            return ToState(BlockInputState, null);
        }

        public bool ToIdleState()
        {
            return ToState(IdleState, null);
        }
        public void RegisterCell(Cell cell)
        {
            if (Cells.TryAdd(cell.Coord, cell))
            {
                cell.OnMouseEnter += HighlightCell;
                cell.OnMouseDown += ClickCell;
            }
        }

        public void UnRegisterCell(Cell cell)
        {
            if (Cells.Remove(cell.Coord))
            {
                cell.OnMouseEnter -= HighlightCell;
                cell.OnMouseDown -= ClickCell;
            }
        }

        /// <summary>
        /// 高亮格子 由于Area2D的MouseEntered发生在MouseExited前 因此不分别绑定事件 在这里进行统一的判定调用
        /// </summary>
        private void HighlightCell(Cell cell)
        {
            if(_highlightCell is not null)
            {
                if (_highlightCell.Unit is null)
                    CurrentGridState.OnCellDehighlighted(_currentUnit, _highlightCell);
                else
                    CurrentGridState.OnUnitDehighlighted(_currentUnit, _highlightCell.Unit);
            }

            _highlightCell = cell;
            if (_highlightCell.Unit is null)
                CurrentGridState.OnCellHighlighted(_currentUnit, _highlightCell);
            else
                CurrentGridState.OnUnitHighlighted(_currentUnit, _highlightCell.Unit);
        }

        private void ClickCell(Cell cell)
        {
            if (cell.Unit is null)
            {
                CurrentGridState.OnCellClicked(_currentUnit, cell);
            }
            else
            {
                CurrentGridState.OnUnitClicked(_currentUnit, cell.Unit);
            }
        }

        public void GenerateCellGrid(int index, LevelData levelData)
        {
            if (index != 0)
                return;

            _tileMap = ResourceLoader.Load<PackedScene>(LevelManager.GetResourcePath(levelData.Map+".tscn")).Instantiate<TileMap>();
            GetTree().CurrentScene.AddChild(_tileMap);
            var cellPrefab = ResourceLoader.Load<PackedScene>(Cell.ResourcePath);
            foreach (var c in _tileMap.GetUsedCells(0))
            {
                var cell = cellPrefab.Instantiate<Cell>();
                _tileMap.AddChild(cell);
                
                cell.Name = $"C{c.X}-{c.Y}";
                cell.Coord = c;
                cell.Position = _tileMap.MapToLocal(c);
                
                RegisterCell(cell);
            }
        }

        public void ClearCellGrid()
        {
            _tileMap.QueueFree();
            _currentUnit = null;
        }
    }
}