using Godot;
using SrpgFramework.Units.Commands;
using SrpgFramework.Ai.Evaluators;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using System.Collections.Generic;
using System.Linq;
using System;

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

        public bool IsAiPlaying { get; private set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
		{
			unit = GetParent<Unit>();

            unit.Ai = this;
            unit.EndExecuteCmd += NextCommand;

            MoveBrains.Add(BattleManager.CommandMgr.GetCommand("move"));

            CellEvaluators.Add(ResourceLoader.Load<Evaluator<Cell>>("res://Resources/Evaluators/RandomCell.tres"));

            IsAiPlaying = false;
        }

        #region Evaluate Code
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
                var score = UnitEvaluators.Sum(evaluator =>
                {
                    return evaluator.Evaluate(toEvaluate, unit) * evaluator.Weight;
                });

                if (UnitScoreDict.ContainsKey(toEvaluate))
                    UnitScoreDict[toEvaluate] = score;
                else
                    UnitScoreDict.Add(toEvaluate, score);
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
                var score = CellEvaluators.Sum(e => e.Evaluate(toEvaluate, unit) * e.Weight);
                if (CellScoreDict.ContainsKey(toEvaluate))
                    CellScoreDict[toEvaluate] = score;
                else
                    CellScoreDict.Add(toEvaluate, score);
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
        #endregion

        public void StartPlay()
        {
            EvaluateCells();
            EvaluateUnits();

            IsAiPlaying = true;
            NextCommand();
        }

        private void NextCommand(string cmdName = "")
        {
            if (IsAiPlaying)
            {
                (Command cmd, float value) top = MoveBrains.Where(c => c.ShouldExecute(unit)).
                    Select(c => (cmd: c, value: c.Evaluate(unit))).OrderByDescending(c => c.value).FirstOrDefault();    //选择一个评分最高的Cmd执行

                if (top.cmd is not null && top.value > 0)   //若无可执行Cmd或Cmd评分<=0则结束Play
                {
                    unit.ExecuteCommand(top.cmd);
                }
                else
                {
                    EndPlay();
                }
            }
        }

        public void EndPlay()
        {
            IsAiPlaying = false;
        }
    }
}