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
        public Action<Cell> OnCellClicked;
        public Action<Cell> OnCellHighlighted;
        public Action<Cell> OnCellDehighlighted;
        public Action<Unit> OnUnitClicked;
        public Action<Unit> OnUnitHighlighted;
        public Action<Unit> OnUnitDeHighlighted;
        public Action<Player> OnTurnStart;
        public Action<Player> OnTurnEnd;

        public virtual void OnMouseEnter()
        {
            if (Unit is null)
            {
                OnCellHighlighted?.Invoke(this);
            }
            else
            {   
                OnUnitHighlighted?.Invoke(Unit);
            }
        }
        public virtual void OnMouseExit()
        {
            if (Unit is null)
                OnCellDehighlighted?.Invoke(this);
            else
                OnUnitDeHighlighted?.Invoke(Unit);
        }
        public virtual void OnMouseDown(Node viewport, InputEvent @event, long shapeIdx)
        {
            if((@event as InputEventMouseButton)?.ButtonIndex == MouseButton.Left && @event.IsPressed())
            {
                if (Unit is null)
                    OnCellClicked?.Invoke(this);
                else
                    OnUnitClicked?.Invoke(Unit);
            }
        }
        #endregion

        #region 高亮        
        public Action<int,string> OnHighlight;

        public void Highlight(int index, string highlighterGroup = CellHighlighter.Group_Default)
        {
            OnHighlight?.Invoke(index, highlighterGroup);
        }

        public void DeHighlight()
        {
            OnHighlight?.Invoke(CellHighlighter.Tag_DeHighlight, CellHighlighter.Group_Default);
        }
        #endregion
    }
}