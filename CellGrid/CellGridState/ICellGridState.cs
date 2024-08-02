using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units;

namespace SrpgFramework.CellGrid.CellGridState
{
    public interface ICellGridState
    {
        public virtual bool CanTranslateTo(ICellGridState nextState)
        {
            return nextState is not null && nextState != this;
        }

        public void OnUnitClicked(Unit self, Unit unit);
        public void OnUnitHighlighted(Unit self, Unit unit);
        public void OnUnitDehighlighted(Unit self, Unit unit);
        public void OnCellDehighlighted(Unit self, Cell cell);
        public void OnCellHighlighted(Unit self, Cell cell);
        public void OnCellClicked(Unit self, Cell cell);
        public void Enter(Unit self);
        public void Exit(Unit self);
    }
}
