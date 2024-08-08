using Godot;
using SrpgFramework.Common;
using System.Collections.Generic;

namespace SrpgFramework.Global
{
	public partial class SceneManager : SingleNode<SceneManager>
    {
        public const string ScenePath = "res://Scene/";
        public const string Scene_Battle = "BattleScene";

        private Dictionary<string, Node> scenes;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
		{
            base._Ready();
            scenes = new();
        }

        public void SwitchScene(string sceneName, bool deleteCurrentScene = false)
        {
            if (!scenes.ContainsKey(sceneName))
            {
                scenes.Add(sceneName, ResourceLoader.Load<PackedScene>(ScenePath + sceneName + ".tscn").Instantiate<Node>());
            }

            if (deleteCurrentScene)
            {
                GetTree().CurrentScene.QueueFree();
            }
            else
            {
                GetTree().Root.RemoveChild(GetTree().CurrentScene);
            }

            GetTree().Root.AddChild(scenes[sceneName]);
            GetTree().CurrentScene = scenes[sceneName];
        }
	}
}