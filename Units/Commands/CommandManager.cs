using Godot;
using System.Collections.Generic;

namespace SrpgFramework.Units.Commands
{
	/// <summary>
	/// 指令系统，例如攻击 移动等 技能也是一类指令
	/// </summary>
	public partial class CommandManager : Node
	{
        private Dictionary<string, Command> skills { get; set; }	//储存获取到的Skill

        public override void _Ready()
		{
            skills = new();
		}

		public static string GetResourcePath(string cmd)
		{
			return $"res://Resources/Commands/{cmd}.tres";
        }

		/// <summary>
		/// 根据Skill Id获取对应技能
		/// </summary>
		public Command GetSkill(string sId)
		{
			if(!skills.ContainsKey(sId))
			{
				skills.Add(sId, ResourceLoader.Load<Command>(GetResourcePath(sId)));
			}
			return skills[sId];
		}
	}
}