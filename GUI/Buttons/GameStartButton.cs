using Godot;
using SrpgFramework.Global;

namespace SrpgFramework.GUI.Buttons
{
	public partial class GameStartButton : Button
	{
        public override void _Ready()
        {
            this.Pressed += () =>
            {
                BattleManager.LevelMgr.LoadLevel("TestLevel");
            };
        }
    }
}