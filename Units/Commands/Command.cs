using SrpgFramework.CellGrid.CellGridState;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using System;
using System.Threading.Tasks;

namespace SrpgFramework.Units.Commands
{
    public abstract class Command :ICellGridState
    {
        public abstract Task Act(Unit unit);

        public async Task Execute(Unit unit, Action preAction, Action postAction)
        {
            preAction();
            await Act(unit);
            postAction();
            await Task.Yield();
        }

        public async Task PlayerExecute(Unit unit)
        {
            await Execute(unit,
                 () => { BattleManager.CellGridMgr.ToBlockInputState(); },
                 () => { BattleManager.CellGridMgr.ToIdleState(); });
        }

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
        public virtual bool CanPerform(Unit self) { return false; }

        public bool ShouldExecute(Unit self) { return ShouldExecute(self, self.Cell); }
        public virtual bool ShouldExecute(Unit unit, Cell cell) { return false; }
        public virtual float Evaluate(Unit unit) { return -1; }
    }
}