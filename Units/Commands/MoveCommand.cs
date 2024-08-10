using SrpgFramework.CellGrid;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Players;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Units.Commands
{
    public partial class MoveCommand : Command
    {
        public override string CmdName => "Move";

        private Cell destination { get; set; }   //目的地

        private IList<Cell> path;   //路径
        private int currentDes;    //移动时的当前目的地
        private HashSet<Cell> moveableArea; //可移动范围

        private float MoveAnimationSpeed = 0.5f;
        
        public override void PreProcess(Unit self)
        {
            self.MovePoints--;

            path = AStar.GetPath(self.Cell, destination, self);
            currentDes = 0;

            BattleManager.CellGridMgr.ToBlockInputState();
        }

        public override void PostProcess(Unit self)
        {
            self.Cell = destination;

            if(BattleManager.PlayerMgr.CurrentPlayer is HumanPlayer)
            {
                BattleManager.CellGridMgr.ToIdleState();
            }
            else
            {
                self.Ai?.EvaluateNeighborCells();
            }
        }

        public override void Process(Unit self, double delta)
        {
            if(self.Position != path[currentDes].Position)
            {
                self.Position = self.Position.MoveToward(path[currentDes].Position, MoveAnimationSpeed);
            }
            else
            {
                currentDes++;
                if(currentDes >= path.Count)
                {
                    self.EndCommand();
                }
            }
        }

        public override void Enter(Unit self)
        {
            moveableArea = AStar.GetMoveableArea(self.Cell, self);
            foreach (var cell in moveableArea)
            {
                cell.Highlight(CellHighlighter.Tag_Selectable);
            }
        }

        public override void Exit(Unit self = null)
        {
            foreach (var cell in moveableArea)
            {
                cell.DeHighlight();
            }
            moveableArea = null;
        }

        public override void OnCellHighlighted(Unit self, Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                path = AStar.GetPath(self.Cell, cell, self);
                foreach (var c in path)
                {
                    c.Highlight(CellHighlighter.Tag_Effect);
                }
            }
            else
                cell.Highlight(CellHighlighter.Tag_Cursor);
        }

        public override void OnCellDehighlighted(Unit self, Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                if (path == null)
                    return;

                foreach (var c in path)
                {
                    c.Highlight(CellHighlighter.Tag_Selectable);
                }
            }
            else
            {
                cell.DeHighlight();
            }
        }

        public override void OnCellClicked(Unit self, Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                destination = cell;
                self.ExecuteCommand(this);
            }
            else
            {
                BattleManager.CellGridMgr.ToIdleState();
            }
        }

        public override void OnUnitHighlighted(Unit self, Unit other)
        {
            other.Highlight(CellHighlighter.Tag_Cursor);
        }

        public override void OnUnitDehighlighted(Unit self, Unit other)
        {
            if (moveableArea.Contains(other.Cell))
                other.Cell.Highlight(CellHighlighter.Tag_Selectable);
            else
                other.DeHighlight();
        }

        public override bool CanAction(Unit self)
        {
            return self.CanMove();
        }

        public override bool ShouldExecute(Unit self, Cell cell)
        {
            if (!CanAction(self))
                return false;

            var top = self.Ai.CellScoreDict.Where(c => self.IsCellMovableTo(c.Key)).OrderByDescending(c => c.Value).First();
            if (top.Value > self.Ai.CellScoreDict[self.Cell])
            {
                destination = top.Key;
                return true;
            }
            return false;
        }

        public override float Evaluate(Unit self)
        {
            var totalPath = AStar.GetPath(self.Cell, destination, self);
            int cost = 0;
            for (var i = 0; i < totalPath.Count; i++)
            {
                cost += totalPath[i].MoveCost;
                if (cost <= self.Mov && self.IsCellMovableTo(totalPath[i]))
                    destination = totalPath[i];
                else
                    break;
            }

            return self.Ai.CellScoreDict[destination];
        }
    }
}