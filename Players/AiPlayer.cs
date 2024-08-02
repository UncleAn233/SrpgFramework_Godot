using SrpgFramework.Global;
using SrpgFramework.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Players
{
    public partial class AiPlayer : Player
    {
        public Action action;

        public override async void Play()
        {
            if (this.Units.Any())
            {
                BattleManager.CellGridMgr.ToBlockInputState();
                foreach (var unit in SelectNextFirstByNearEnemy())
                {
                    await unit.Ai?.Execute();
                }
                BattleManager.PlayerMgr.NextPlayer();
                return;
            }
        }

        private void OnGameEnded()
        {
        }

        private IEnumerable<Unit> SelectNextFirstByNearEnemy()
        {
            return this.Units.OrderBy(unit =>
            {
                return BattleManager.UnitMgr.GetEnemyUnits(this).Min(enemy => unit.Cell.GetDistance(enemy.Cell)); //离敌人近的先动
            });
        }
    }
}
