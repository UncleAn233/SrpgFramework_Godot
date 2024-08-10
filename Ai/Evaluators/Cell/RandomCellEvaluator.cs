using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units;
using System;

namespace SrpgFramework.Ai.Evaluators
{
    public partial class RandomCellEvaluator : Evaluator<Cell>
    {
        public override float Evaluate(Cell toEvaluate, Unit unit)
        {
            if (unit.Cell.GetDistance(toEvaluate) == unit.Mov)
            {
                return 1;
            }
            else
                return 0;
        }
    }
}