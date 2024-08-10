using Godot;
using SrpgFramework.Players;
using System;

namespace SrpgFramework.CellGrid.Cells
{
    // 本部分为Cell的事件代码
    public partial class Cell
    {
        #region GUI
        public Action<Cell> MouseEnter;
        public Action<Cell> MouseDown;
        public Action<Player> TurnStart;
        public Action<Player> TurnEnd;

        public virtual void OnMouseEnter()
        {
            MouseEnter?.Invoke(this);
        }

        public virtual void OnMouseDown(Node viewport, InputEvent @event, long shapeIdx)
        {
            if((@event as InputEventMouseButton)?.ButtonIndex == MouseButton.Left && @event.IsPressed())
            {
                MouseDown?.Invoke(this);
            }
        }
        #endregion

        #region 高亮        
        public Action<int> OnHighlight;

        public void Highlight(int index)
        {
            OnHighlight?.Invoke(index);
        }

        public void DeHighlight()
        {
            OnHighlight?.Invoke(CellHighlighter.Tag_DeHighlight);
        }
        #endregion
    }
}