using System.Collections.Generic;

namespace SrpgFramework.Units
{
    public interface IUnitData
    {
        string Id { get; }
        /// <summary>
        /// ��ǩ
        /// </summary>
        HashSet<string> Tags { get; }

        /// <summary>
        /// �ȼ�
        /// </summary>
        public int Lv { get; }

        /// <summary>
        /// Ѫ��
        /// </summary>
        int Hp { get; }

        /// <summary>
        /// ����
        /// </summary>
        int Atk { get; }
        /// <summary>
        /// ����
        /// </summary>
        int Def { get; }
        /// <summary>
        /// ħ��
        /// </summary>
        int Mdef { get; }
        /// <summary>
        /// ����
        /// </summary>
        int Dodge { get; }
        /// <summary>
        /// �ƶ�
        /// </summary>
        int Mov { get; }
        /// <summary>
        /// ���
        /// </summary>
        int Hate { get; }
    }
}