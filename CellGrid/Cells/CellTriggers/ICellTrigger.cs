
namespace SrpgFramework.CellGrid.Cells
{
    public interface ICellTrigger
    {
        void Register(Cell cell);
        void Unregister(Cell cell);
    }
}