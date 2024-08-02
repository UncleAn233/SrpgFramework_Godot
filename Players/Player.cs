using Godot;
using SrpgFramework.Global;
using SrpgFramework.Units;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Players
{
    public abstract partial class Player : Node
    {
        public int PlayerNumber { get; set; }
        public PlayerAlignment Alignment { get; set; }

        public List<Unit> Units => BattleManager.UnitMgr.Units.Where(u => u.PlayerNumber == PlayerNumber).ToList();
        public abstract void Play();

        public bool IsFriend(Player player)
        {
            return player is not null && this.Alignment == player.Alignment;
        }
        public bool IsEnemy(Player player)
        {
            return player is not null && (this.Alignment & player.Alignment) == 0;
        }
        public bool HasUnit()
        {
            return BattleManager.UnitMgr.Units.Any(u => u.PlayerNumber == PlayerNumber);
        }
    }
        public enum PlayerAlignment
        {
            Friend = 0b_01,
            Enemy = 0b_10,
            Third = 0b_11,
        }
    
}