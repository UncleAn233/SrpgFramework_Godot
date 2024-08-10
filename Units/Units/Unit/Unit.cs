using Godot;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Players;
using SrpgFramework.Units.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Units
{
    public partial class Unit : Node2D
    {
        public const string ResourcePath = "res://Resources/Prefabs/unit.tscn";

        /// <summary>
        /// 回合开始
        /// </summary>
        public Action<int> TurnStart;
        /// <summary>
        /// 回合结束
        /// </summary>
        public Action<int> TurnEnd;
        /// <summary>
        /// HP变化
        /// </summary>
        public Action<int> HpUpdated;
        /// <summary>
        /// MP变化
        /// </summary>
        public Action<int> MpUpdated;
        /// <summary>
        /// Hp低于0时
        /// </summary>
        public Action Die;
        /// <summary>
        /// 受到伤害前
        /// </summary>
        public Action BeforeDamaged;
        /// <summary>
        /// 受到伤害后
        /// </summary>
        public Action AfterDamaged;

        /// <summary>
        /// 开始执行指令
        /// </summary>
        public Action<string> StartExecuteCmd;
        /// <summary>
        /// 执行指令结束
        /// </summary>
        public Action<string> EndExecuteCmd;

        /// <summary>
        /// 决定读取的Unit数据
        /// </summary>
        
        /// <summary>
        /// 所属Player编号
        /// </summary>
        public int PlayerNumber { get; set; }
        public Player Player => BattleManager.PlayerMgr.GetPlayer(PlayerNumber);

        /// <summary>
        /// 行动点数
        /// </summary>
        public int ActionPoints { get; set; } = 1;
        public int TotalActionPoints { get; set; } = 1;
        /// <summary>
        /// 移动点数
        /// </summary>
        public int MovePoints { get; set; } = 1;
        public int TotalMovePoints { get; set; } = 1;

        public UnitType UnitType { get; private set; }

        private Cell cell;
        public Cell Cell
        {
            get { return cell; }
            set
            {
                if (cell is not null)
                {
                    cell.Unit = null;
                }
                cell = value;
                cell.Unit = this;
                this.Position = cell.Position;
            }
        }

        public bool a;
        private bool isExecutingCmd;
        private Command executingCmd { get; set; } //当前正在执行的指令

        public AiUnit Ai { get; internal set; }
        
        public override void _Ready()
        {
            BattleManager.PlayerMgr.RegisterUnit(this);
            
            TurnEnd += (t) =>
            {
                ActionPoints = TotalActionPoints;
                MovePoints = TotalMovePoints;
            };
        }

        public override void _Process(double delta)
        {
            if(isExecutingCmd)
                executingCmd.Process(this, delta);
        }
        public override void _PhysicsProcess(double delta)
        {
            if (isExecutingCmd)
                executingCmd.PhysicsProcess(this, delta);
        }

        /// <summary>
        /// 执行Cmd
        /// </summary>
        /// <param name="command"></param>
        public void ExecuteCommand(Command command)
        {
            if (isExecutingCmd)
                return;

            command.PreProcess(this);
            StartExecuteCmd?.Invoke(command.CmdName);

            isExecutingCmd = true;
            executingCmd = command;
        }

        /// <summary>
        /// 结束Cmd
        /// </summary>
        public void EndCommand()
        {
            executingCmd.PostProcess(this);
            EndExecuteCmd?.Invoke(executingCmd.CmdName);

            isExecutingCmd = false;
            executingCmd = null;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(string id)
        {
            Id = id;
            if (UnitType == UnitType.Character)
            {
                this.AddChild(new AiUnit());
            }
        }

        public void OnTurnStart(int turn)
        {
            TurnStart?.Invoke(turn);
        }
        public void OnTurnEnd(int turn)
        {
            TurnEnd?.Invoke(turn);
        }

        #region 移动相关代码
        public bool CanMove()
        {
            return MovePoints > 0;
        }

        public virtual bool IsCellMovableTo(Cell cell)
        {
            return !cell.IsTaken && (Cell.GroundType & MovableGroundType) > 0;
        }
        public virtual bool IsCellTraversable(Cell cell)
        {
            if ((Cell.GroundType & MovableGroundType) == 0)
            {
                return false;
            }
            else
            {
                return !cell.IsTaken || cell.Unit.Player.IsFriend(cell.Unit.Player);
            }
        }
        #endregion
    }
    public enum UnitType
    {
        Character,Building
    }
}