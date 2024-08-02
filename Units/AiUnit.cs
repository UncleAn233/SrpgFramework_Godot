using Godot;
using SrpgFramework.Units.Commands;
using SrpgFramework.Ai.Evaluators;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SrpgFramework.Units
{
	public partial class AiUnit : Node
	{
        private Unit unit;
        public Dictionary<Unit, float> UnitScoreDict { get; private set; } = new();
        public Dictionary<Cell, float> CellScoreDict { get; private set; } = new();

        public HashSet<Command> MoveBrains { get; private set; } = new();
        public HashSet<Command> ActionBrains { get; private set; } = new();

        public List<Evaluator<Unit>> UnitEvaluators = new();
        public List<Evaluator<Cell>> CellEvaluators = new();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
		{
			unit = GetParent<Unit>();
            unit.Ai = this;
		}

        //评估
        public void EvaluateUnits()
        {
            UnitScoreDict.Clear();

            foreach (var u in BattleManager.UnitMgr.Units)
            {
                EvaluateUnit(u);
            }
        }

        public void EvaluateCells()
        {
            CellScoreDict.Clear();
            foreach (var c in BattleManager.CellGridMgr.Cells.Values)
            {
                EvaluateCell(c);
            }
        }

        public void EvaluateUnit(Unit toEvaluate)
        {
            if (UnitEvaluators.Any())
            {
                UnitScoreDict.Add(toEvaluate, UnitEvaluators.Sum(evaluator =>
                {
                    evaluator.PreCalculate(unit);
                    return evaluator.Evaluate(toEvaluate, unit) * evaluator.Weight;
                }));
            }
            else
            {
                UnitScoreDict.Add(toEvaluate, 0);
            }
        }

        public void EvaluateCell(Cell toEvaluate)
        {
            if (CellEvaluators.Any())
            {
                CellScoreDict.Add(toEvaluate, CellEvaluators.Sum(evaluator =>
                {
                    evaluator.PreCalculate(unit);
                    return evaluator.Evaluate(toEvaluate, unit) * evaluator.Weight;
                }));
            }
            else
            {
                CellScoreDict.Add(toEvaluate, 0);
            }
        }

        /// <summary>
        /// 评估周围一圈Cell 默认为单位可移动范围
        /// </summary>
        public void EvaluateNeighborCells()
        {
            var cells = unit.Cell.GetNeighborCells(unit.Mov);
            foreach (var cell in cells)
            {
                EvaluateCell(cell);
            }
        }

        //实行
        public async Task Execute()
        {
            EvaluateUnits();
            var brainList = MoveBrains.Concat(ActionBrains);
            var brains = brainList.Where(brain => brain.ShouldExecute(unit, unit.Cell));

            while (brains.Any())
            {
                var topBrain = brains.OrderByDescending(brain => brain.Evaluate(unit)).First();
                await topBrain.AIExecute(unit);
                brains = brainList.Where(brain => brain.ShouldExecute(unit, unit.Cell));
            }
            await Task.Yield();
        }
    }
}