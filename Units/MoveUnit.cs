using Godot;
using SrpgFramework.CellGrid.Cells;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SrpgFramework.Units
{
	public partial class MoveUnit : Node
	{
        private Unit unit;

        public GroundType MovableGroundType { get; set; } = GroundType.Ground;

        public static float MoveAnimationSpeed = 1.5f;

        public int MovePoints { get; private set; } = 1;
        public int TotalMovePoints { get; private set; } = 1;

        public Action OnMoveStart;
        public Action OnMoveEnd;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            unit = GetParent<Unit>();
            unit.Move = this;

            unit.OnTurnEnd += (t) => { MovePoints = TotalMovePoints; };
        }

        public bool CanMove()
        {
            return MovePoints > 0;
        }

        public virtual bool IsCellMovableTo(Cell cell)
        {
            return cell.IsTaken && (unit.Cell.GroundType & MovableGroundType) > 0;
        }
        public virtual bool IsCellTraversable(Cell cell)
        {
            if ((unit.Cell.GroundType & MovableGroundType) == 0)
            {
                return false;
            }
            else
            {
                return !cell.IsTaken || cell.Unit.Player.IsFriend(cell.Unit.Player);
            }
        }

        public async Task Move(Cell destinition, IList<Cell> path)
        {
            MovePoints--;
            OnMoveStart?.Invoke();

            if (MoveAnimationSpeed > 0)
            {
                foreach (var cell in path)
                {
                    var destination_pos = cell.Position;
                    //Face();
                    while (unit.Position != destination_pos)
                    {
                        await Task.Run(() => unit.Position.MoveToward(destination_pos, MoveAnimationSpeed));
                        
                    }
                }
            }
            else
            {
                unit.Position = destinition.Position;
            }
            unit.Cell = destinition;
            OnMoveEnd?.Invoke();
        }
	}
}