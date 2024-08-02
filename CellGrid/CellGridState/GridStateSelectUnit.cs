using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units;
using System.Collections.Generic;

namespace SrpgFramework.CellGrid.CellGridState
{
    public class GridStateSelectUnit : ICellGridState
    {
        private CellGridManager _mgr;

        private HashSet<Cell> moveableArea;
        private List<Cell> path;
        public GridStateSelectUnit(CellGridManager mgr)
        {
            _mgr = mgr;
        }

        public void Enter(Unit self)
        {
            moveableArea = AStar.GetMoveableArea(self.Cell, self.Move, self.Mov);
            foreach (var c in moveableArea)
            {
                c.Highlight(CellHighlighter.Tag_Selectable);
            }
        }

        public void Exit(Unit self)
        {
        }

        public void OnCellClicked(Unit self, Cell cell)
        {
            _mgr.ToIdleState();
        }

        public void OnCellHighlighted(Unit self, Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                path = AStar.GetPath(self.Cell, cell, self.Move);
                foreach (var c in path)
                    c.Highlight(CellHighlighter.Tag_Effect);
            }
            else
            {
                cell.Highlight(CellHighlighter.Tag_Selectable);
            }
        }

        public void OnCellDehighlighted(Unit self, Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                foreach (var c in path)
                    c.Highlight(CellHighlighter.Tag_Selectable);
            }
            else
            {
                cell.DeHighlight();
            }
        }

        public void OnUnitClicked(Unit self, Unit unit)
        {
        }

        public void OnUnitDehighlighted(Unit self, Unit unit)
        {
            unit.DeHighlight();
        }

        public void OnUnitHighlighted(Unit self, Unit unit)
        {
            unit.Highlight(CellHighlighter.Tag_Cursor);
        }
    }
}