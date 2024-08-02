using Godot;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Players;

namespace SrpgFramework.Units
{
    public partial class Unit : Node2D
    {
        private string _id;
        public string ID
        {
            get => _id;
            set
            {
                if (_id is null)
                {
                    _id = value;
                }
            }
        }
        public int PlayerNumber { get; set; }
        public Player Player => BattleManager.PlayerMgr.GetPlayer(PlayerNumber);

        public int ActionPoint { get; set; } = 1;
        public int TotalActionPoint { get; set; } = 1;

        public UnitType UnitType { get; private set; }

        private Cell cell;
        public Cell Cell
        {
            get { return cell; }
            set
            {
                if (cell is not null)
                {
                    cell.Unit = null;
                }
                cell = value;
                cell.Unit = this;
            }
        }

        public MoveUnit Move { get; internal set; }
        public AiUnit Ai { get; internal set; }

        public override void _Ready()
        {
            if(UnitType == UnitType.PC || UnitType == UnitType.NPC)
            {
                this.AddChild(new MoveUnit());
                this.AddChild(new AiUnit());
            }
            Cell = BattleManager.CellGridMgr.Cells[new Vector2I(8, 8)];
        }
        public void Init()
        {

        }

        public void SetPos(Cell cell)
        {
            if (cell is not null)
            {
                Cell = cell;
                this.Position = cell.Position;
            }
        }
    }
    public enum UnitType
    {
        PC,NPC,NC
    }
}