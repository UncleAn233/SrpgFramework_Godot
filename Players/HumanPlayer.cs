using SrpgFramework.Global;

namespace SrpgFramework.Players
{
    public partial class HumanPlayer : Player
    {
        public override void Play()
        {
            BattleManager.CellGridMgr.ToIdleState();
        }
    }
}