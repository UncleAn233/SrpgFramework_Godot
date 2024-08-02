using Godot;
using SrpgFramework.Units.Commands;
using SrpgFramework.CellGrid;
using SrpgFramework.Level;
using SrpgFramework.Players;
using SrpgFramework.Units;

namespace SrpgFramework.Global
{
	public partial class BattleManager : Node
	{
        private static BattleManager instance;
        public static BattleManager Instance { get => instance; }

        public static LevelManager LevelMgr { get; private set; }
        public static CellGridManager CellGridMgr { get; private set; }
        public static UnitManager UnitMgr { get; private set; }
        public static CommandManager CommandMgr { get; private set; }
        public static PlayerManager PlayerMgr { get; private set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
		{
            if(instance is not null)
            {
                this.QueueFree();
                return;
            }

            instance = this;

            this.AddChild(LevelMgr = new LevelManager());
            this.AddChild(CellGridMgr = new CellGridManager());
            this.AddChild(UnitMgr = new UnitManager());
            this.AddChild(CommandMgr = new CommandManager());
            this.AddChild(PlayerMgr = new PlayerManager());
        }
	}
}