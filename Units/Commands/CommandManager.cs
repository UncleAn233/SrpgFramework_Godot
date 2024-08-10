using Godot;
using System.Collections.Generic;

namespace SrpgFramework.Units.Commands
{
	/// <summary>
	/// 指令系统，例如攻击 移动等 技能也是一类指令
	/// </summary>
	public partial class CommandManager : Node
	{
        private Dictionary<string, Command> commands { get; set; }	//储存获取到的Command

        public override void _Ready()
		{
            commands = new();
		}

		public static string GetResourcePath(string cmd)
		{
			return $"res://Resources/Commands/{cmd}.tres";
        }

		public Command GetCommand(string cmd)
		{
			if(!commands.ContainsKey(cmd))
			{
                commands.Add(cmd, ResourceLoader.Load<Command>(GetResourcePath(cmd)));
			}
			return commands[cmd];
		}
	}
}