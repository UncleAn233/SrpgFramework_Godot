using SrpgFramework.CellGrid.Cells;
using System.Collections.Generic;

namespace SrpgFramework.Units
{
    public partial class Unit : IUnitData  //单位属性 面板
    {
        public string Id { get; private set; }
        public HashSet<string> Tags { get; set; }   

        public int Lv { get; set; }    //等级

        public int MaxHp { get; set; }
        private int hp;
        public int Hp
        {
            get => hp;
            set
            {
                hp = value;
                OnHpUpdate?.Invoke(hp);
                if (hp < 1)
                    this.OnDie?.Invoke();
            }
        }

        public int Atk { get; set; }
        public int Def { get; set; }
        public int Mdef { get; set; }
        public int Dodge { get; set; }
        public int Mov { get; set; } = 3;
        public int Hate { get; set; }

        public GroundType MovableGroundType { get; set; } = GroundType.Ground;  //可移动地块类型

        /// <summary>
        /// Hp百分比
        /// </summary>
        public float HpPercent { get => (float)hp / MaxHp; }
    }
}