using Godot;

namespace SrpgFramework.CellGrid.Cells
{
    public partial class CellHighlighter : AnimatedSprite2D, ICellHighlighter
    {
        public const int Tag_DeHighlight = 0;
        public const int Tag_Cursor = 1;
        public const int Tag_Selectable = 2;
        public const int Tag_Effect = 3;

        public override void _Ready()
        {
            var cell = GetParent<Cell>();
            cell.OnHighlight += Apply;
        }

        public void Apply(int index)
        {
            this.Frame = index;
        }
    }
}