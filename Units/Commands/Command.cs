using Godot;
using SrpgFramework.CellGrid.CellGridState;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using System;
using System.Threading.Tasks;

namespace SrpgFramework.Units.Commands
{
    public abstract partial class Command :Resource, ICellGridState 
    {
        /// <summary>
        /// 指令具体效果
        /// </summary>
        /// <param name="unit"></param>
        public abstract void Act(Unit unit);

        /// <summary>
        /// 执行Act
        /// </summary>
        /// <param name="preAction">执行前状态</param>
        /// <param name="postAction">执行后状态</param>
        /// <returns></returns>
        public async Task Execute(Unit unit, Action preAction, Action postAction)
        {
            preAction();
            Act(unit);
            postAction();
            await Task.Yield();
        }

        /// <summary>
        /// 人类执行 执行前关闭输入 执行后打开
        /// </summary>
        public async Task PlayerExecute(Unit unit)
        {
            await Execute(unit,
                 () => { BattleManager.CellGridMgr.ToBlockInputState(); },
                 () => { BattleManager.CellGridMgr.ToIdleState(); });
        }

        /// <summary>
        /// AI执行 执行完毕后重新评估
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public virtual async Task AIExecute(Unit unit)
        {
            await Execute(unit, () => { },
                () =>
                {
                    unit.Ai.EvaluateUnits();
                    unit.Ai.EvaluateNeighborCells();
                });
        }

        public virtual void Enter(Unit self) { }
        public virtual void Exit(Unit self) { }
        public virtual void OnCellClicked(Unit self, Cell cell) { }
        public virtual void OnCellDehighlighted(Unit self, Cell cell) { }
        public virtual void OnCellHighlighted(Unit self, Cell cell) { }
        public virtual void OnUnitClicked(Unit self, Unit unit) { }
        public virtual void OnUnitDehighlighted(Unit self, Unit unit) { }
        public virtual void OnUnitHighlighted(Unit self, Unit unit) { }
        /// <summary>
        /// 指令是否可以执行
        /// </summary>
        public virtual bool CanAction(Unit self) { return false; }

        
        public bool ShouldExecute(Unit self) { return ShouldExecute(self, self.Cell); }
        /// <summary>
        /// 为否时 AI不会考虑执行该指令
        /// </summary>
        public virtual bool ShouldExecute(Unit unit, Cell cell) { return false; }
        /// <summary>
        /// 指令评分
        /// </summary>
        public virtual float Evaluate(Unit unit) { return -1; }
    }
}