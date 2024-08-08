using Godot;
using SrpgFramework.CellGrid.Cells;
using System;
namespace SrpgFramework.Units.Commands
{
	public class SkillData
	{
		/// <summary>
		/// 技能ID
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 射程
		/// </summary>
		public int Range { get; set; }
		/// <summary>
		/// 影响范围 形状
		/// </summary>
		public AreaShape EffectShape { get; set; }
		/// <summary>
		/// 影响范围 距离
		/// </summary>
		public int EffectRange { get; set; }
		/// <summary>
		/// 消耗mp
		/// </summary>
		public int MpCost { get; set; }
		/// <summary>
		/// 消耗hp
		/// </summary>
		public int HpCost { get;set; }
	
		public int Power { get; set; }
	}
}