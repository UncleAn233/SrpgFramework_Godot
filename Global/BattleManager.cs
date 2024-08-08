using SrpgFramework.Units.Commands;
using SrpgFramework.CellGrid;
using SrpgFramework.Level;
using SrpgFramework.Players;
using SrpgFramework.Units;
using SrpgFramework.Common;

namespace SrpgFramework.Global
{
	public partial class BattleManager : SingleNode<BattleManager>
	{
        public static LevelManager LevelMgr { get; private set; }
        public static CellGridManager CellGridMgr { get; private set; }
        public static UnitManager UnitMgr { get; private set; }
        public static CommandManager CommandMgr { get; private set; }
        public static PlayerManager PlayerMgr { get; private set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
		{
            base._Ready();

            this.AddChild(LevelMgr = new LevelManager());
            LevelMgr.Name = nameof(LevelMgr);
            this.AddChild(PlayerMgr = new PlayerManager());
            PlayerMgr.Name = nameof(PlayerMgr);
            this.AddChild(CellGridMgr = new CellGridManager());
            CellGridMgr.Name = nameof(CellGridMgr);
            this.AddChild(UnitMgr = new UnitManager());
            UnitMgr.Name = nameof(UnitMgr);
            this.AddChild(CommandMgr = new CommandManager());
            CommandMgr.Name = nameof(CommandMgr);
        }
	}
}