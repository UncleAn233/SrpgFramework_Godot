using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units;
using SrpgFramework.Units.Commands;

namespace SrpgFramework.CellGrid.CellGridState
{
    public class GridStateIdle : ICellGridState
    {
        private CellGridManager _mgr;

        private MoveCommand move = new();
        public GridStateIdle(CellGridManager mgr)
        {
            this._mgr = mgr;
        }

        public void Enter(Unit self)
        {
        }

        public void Exit(Unit self)
        {
            
        }

        public void OnUnitClicked(Unit self, Unit unit)
        {
            //_mgr.ToState(_mgr.SelectUnitState, unit);
            _mgr.ToState(move, unit);
        }

        public void OnUnitHighlighted(Unit self, Unit unit)
        {
            unit.Highlight(CellHighlighter.Tag_Cursor);
        }

        public void OnUnitDehighlighted(Unit self, Unit unit)
        {
            unit.DeHighlight();
        }

        public void OnCellDehighlighted(Unit self, Cell cell)
        {
            cell.DeHighlight();
        }

        public void OnCellHighlighted(Unit self, Cell cell)
        {
            cell.Highlight(CellHighlighter.Tag_Cursor);
        }

        public void OnCellClicked(Unit self, Cell cell)
        {
        }
    }
}