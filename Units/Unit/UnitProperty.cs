using System.Collections.Generic;

namespace SrpgFramework.Units
{
    public partial class Unit   //单位属性 面板
	{
        public HashSet<string> Tags;

        public int Lv { get; set; }    //等级
        public int Exp { get; set; }     //经验

        public int MaxHp { get; set; }
        private int hp;    //血条
        public int Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp < 1)
                    this.OnDie?.Invoke();
                else
                    OnHpUpdate?.Invoke(hp);
            }
        }

        public int Atk { get; set; }   //攻击
        public int Def { get; set; }    //防御
        public int Mdef { get; set; }   //魔抗
        public int Dodge { get; set; }   //闪避
        public int Mov { get; set; } = 3;   //移动
        public int Hate { get; set; } //仇恨

        public float HpPercent { get => (float)hp / MaxHp; }
    }
}