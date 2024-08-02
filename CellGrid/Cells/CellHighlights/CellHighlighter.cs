using Godot;

namespace SrpgFramework.CellGrid.Cells
{
	public partial class CellHighlighter : AnimatedSprite2D
    {
        public const string Group_Default = "default";
        public const string Group_Path = "path";

        public const int Tag_DeHighlight = 0;
        public const int Tag_Cursor = 1;
        public const int Tag_Selectable = 2;
        public const int Tag_Effect = 3;

        public override void _Ready()
        {
            var cell = GetParent<Cell>();
            cell.OnHighlight += Apply;
        }

        public void Apply(int index, string highlighterGroup)
        {
            this.Animation = highlighterGroup;
            this.Frame = index;
        }
    }
}