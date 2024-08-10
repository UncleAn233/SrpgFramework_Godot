using Godot;
using SrpgFramework.Global;
using SrpgFramework.Level;
using SrpgFramework.Players;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Units
{
	public partial class UnitManager : Node
	{
        public HashSet<Unit> Units { get; private set; }

        private PackedScene unitPrefab;

        public override void _Ready()
        {
            Units = new();
            unitPrefab = ResourceLoader.Load<PackedScene>(Unit.ResourcePath);
            BattleManager.LevelMgr.OnLevelLoad += GenerateUnits;
        }

        public HashSet<Unit> GetEnemyUnits(Player player)
        {
            return Units.Where(u => u.Player.IsEnemy(player)).ToHashSet();
        }
        
        /// <summary>
        /// 根据关卡文件配置Units
        /// </summary>
        public void GenerateUnits(int index, LevelData levelData)
        {
            if (index != 1)
                return;

            var unitParent = GetTree().CurrentScene.GetNodeOrNull("Units");
            if(unitParent is null)
            {
                unitParent = new Node();
                unitParent.Name = "Units";
                GetTree().CurrentScene.AddChild(unitParent);
            }

            foreach(var unit in levelData.Units)
            {
                NewUnit(unit.Id, unit.Player, unit.Cell, unitParent);
            }
        }

        /// <summary>
        /// 创建一个新Unit
        /// </summary>
        public void NewUnit(string id, int player, Vector2I cell, Node parent)
        {
            var unit = unitPrefab.Instantiate<Unit>();
            parent.AddChild(unit);
            unit.Init(id);
            unit.PlayerNumber = player;
            unit.Cell = BattleManager.CellGridMgr.Cells[cell];

            Units.Add(unit);
        }
	}
}