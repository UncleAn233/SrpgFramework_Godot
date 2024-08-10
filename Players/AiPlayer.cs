using SrpgFramework.Global;
using SrpgFramework.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Players
{
    public partial class AiPlayer : Player
    {
        public Action PlayEnd;

        private bool isPlaying;

        private IEnumerator<Unit> unitLine;

        public override void _Ready()
        {
            isPlaying = false;
        }

        public override void Play()
        {
            unitLine = SelectUnitFirstByNearEnemy().GetEnumerator();
            
            if(unitLine.MoveNext())
            {
                isPlaying = true;
                unitLine.Current.Ai.StartPlay();
            }
        }

        public override void _Process(double delta)
        {
            if(isPlaying && !unitLine.Current.Ai.IsAiPlaying)
            {
                if(unitLine.MoveNext())
                {
                    unitLine.Current.Ai.StartPlay();
                }
                else
                {
                    isPlaying = false;
                    unitLine = null;
                    BattleManager.PlayerMgr.NextPlayer();
                }
            }
        }

        private void OnGameEnded()
        {
        }


        private IEnumerable<Unit> SelectUnitFirstByNearEnemy()
        {
            return this.Units.Where(u => u.Ai is not null).OrderBy(unit =>
            {
                return BattleManager.UnitMgr.GetEnemyUnits(this).Min(enemy => unit.Cell.GetDistance(enemy.Cell));
            });
        }
    }
}
