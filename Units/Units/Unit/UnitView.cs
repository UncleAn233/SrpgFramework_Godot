using SrpgFramework.CellGrid.Cells;

namespace SrpgFramework.Units
{
    public partial class Unit
    {
        public void Highlight(int index)
        {
            this.Cell.Highlight(index);
        }

        public void DeHighlight()
        {
            this.cell.DeHighlight();
        }
    }
}