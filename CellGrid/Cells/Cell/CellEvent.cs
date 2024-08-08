using Godot;
using SrpgFramework.Players;
using SrpgFramework.Units;
using System;

namespace SrpgFramework.CellGrid.Cells
{
    // 本部分为Cell的事件代码
    public partial class Cell
    {
        #region GUI
        public Action<Cell> OnMouseEnter;
        public Action<Cell> OnMouseDown;
        public Action<Player> OnTurnStart;
        public Action<Player> OnTurnEnd;

        public virtual void MouseEnter()
        {
            OnMouseEnter?.Invoke(this);
        }

        public virtual void MouseDown(Node viewport, InputEvent @event, long shapeIdx)
        {
            if((@event as InputEventMouseButton)?.ButtonIndex == MouseButton.Left && @event.IsPressed())
            {
                OnMouseDown?.Invoke(this);
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