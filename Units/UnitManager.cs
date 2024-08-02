using Godot;
using SrpgFramework.Players;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Units
{
	public partial class UnitManager : Node
	{
        public HashSet<Unit> Units { get; private set; }
        public override void _Ready()
        {
            Units = new();
        }

        public HashSet<Unit> GetEnemyUnits(Player player)
        {
            return Units.Where(u => u.Player.IsEnemy(player)).ToHashSet();
        }
	}
}