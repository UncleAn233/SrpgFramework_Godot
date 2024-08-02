using System;

namespace SrpgFramework.Units
{
	public partial class Unit
	{
        public Action<int> OnTurnStart;
        public Action<int> OnTurnEnd;
        public void TurnStart(int turn)
        {
            OnTurnStart?.Invoke(turn);
        }
        public void TurnEnd(int turn)
        {
            OnTurnEnd?.Invoke(turn);
        }

        public Action<int> OnHpUpdate;
        public Action<int> OnMpUpdate;
        /// <summary>
        /// Hp低于0时
        /// </summary>
        public Action OnDie;

        public Action BeforeDamaged;
        public Action AfterDamaged;
    }
}