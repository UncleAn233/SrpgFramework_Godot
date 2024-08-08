using static SrpgFramework.Common.Constatnts;
using SrpgFramework.CellGrid;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Units.Commands
{
    public partial class MoveCommand : Command
    {
        public Cell Destination { get; set; }   //目的地
        private IList<Cell> path;   //路径
        private HashSet<Cell> moveableArea; //可移动范围

        private float MoveAnimationSpeed = 0.5f;

        public override void Act(Unit unit)
        {
            path = AStar.GetPath(unit.Cell, Destination, unit);
            Move(unit, Destination, path);
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
            path = null;
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

        public override async void OnCellClicked(Unit self, Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                Destination = cell;
                await PlayerExecute(self);
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
                Destination = top.Key;
                return true;
            }
            return false;
        }

        public override float Evaluate(Unit self)
        {
            var totalPath = AStar.GetPath(self.Cell, Destination, self);
            int cost = 0;
            for (var i = 0; i < totalPath.Count; i++)
            {
                cost += totalPath[i].MoveCost;
                if (cost <= self.Mov && self.IsCellMovableTo(totalPath[i]))
                    Destination = totalPath[i];
                else
                    break;
            }

            return self.Ai.CellScoreDict[Destination];
        }

        public async void Move(Unit unit, Cell destinition, IList<Cell> path)
        {
            unit.MovePoints--;
            unit.OnMoveStart?.Invoke();

            if (MoveAnimationSpeed > 0)
            {
                foreach (var cell in path)
                {
                    var destination_pos = cell.Position;
                    //Face();
                    while (unit.Position != destination_pos)
                    {
                        unit.Position = unit.Position.MoveToward(destination_pos, MoveAnimationSpeed);
                        await ToSignal(unit.GetTree(), ProcessFrame_Single);
                    }
                }
            }
            else
            {
                unit.Position = destinition.Position;
            }
            unit.Cell = destinition;
            unit.OnMoveEnd?.Invoke();
        }
    }
}