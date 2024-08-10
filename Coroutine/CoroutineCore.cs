using SrpgFramework.Common;
using System.Collections;
using System.Collections.Generic;

namespace SrpgFramework.Coroutine
{
	/// <summary>
	/// 类似U3D的协程实现
	/// </summary>
	public partial class CoroutineCore : SingleNode<CoroutineCore>
	{
		private static Stack<IEnumerator> s_Its = new();

		public static void StartCoroutine(IEnumerator ie)
		{
			s_Its.Push(ie);
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}