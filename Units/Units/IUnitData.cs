using System.Collections.Generic;

namespace SrpgFramework.Units
{
    public interface IUnitData
    {
        string Id { get; }
        /// <summary>
        /// ±êÇ©
        /// </summary>
        HashSet<string> Tags { get; }

        /// <summary>
        /// µÈ¼¶
        /// </summary>
        public int Lv { get; }

        /// <summary>
        /// ÑªÁ¿
        /// </summary>
        int Hp { get; }

        /// <summary>
        /// ¹¥»÷
        /// </summary>
        int Atk { get; }
        /// <summary>
        /// ·ÀÓù
        /// </summary>
        int Def { get; }
        /// <summary>
        /// Ä§¿¹
        /// </summary>
        int Mdef { get; }
        /// <summary>
        /// ÉÁ±Ü
        /// </summary>
        int Dodge { get; }
        /// <summary>
        /// ÒÆ¶¯
        /// </summary>
        int Mov { get; }
        /// <summary>
        /// ³ðºÞ
        /// </summary>
        int Hate { get; }
    }
}