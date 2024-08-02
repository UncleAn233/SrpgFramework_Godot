using SrpgFramework.CellGrid.Cells;

namespace SrpgFramework.Units
{
    public partial class Unit
    {
        public void Highlight(int index, string highlightGroud = CellHighlighter.Group_Default)
        {
            this.Cell.Highlight(index, highlightGroud);
        }

        public void DeHighlight()
        {
            this.cell.DeHighlight();
        }
    }
}