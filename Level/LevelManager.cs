using Godot;
using SrpgFramework.Global;
using System;

namespace SrpgFramework.Level
{
	public partial class LevelManager : Node
	{
		public Action<int, LevelData> OnLevelLoad;
		public Action OnLevelStart;
		public Action OnLevelEnd;

		public void LoadLevel(string level)
		{
            SceneManager.Instance.SwitchScene(SceneManager.Scene_Battle);

            var json = ResourceLoader.Load<Json>(GetResourcePath(level+".json")).Data.ToString();
			var levelData = new LevelData(json);
			
			OnLevelLoad(0, levelData);
            OnLevelLoad(1, levelData);
        }

		public static string GetResourcePath(string str)
		{
			return $"res://Resources/Level/{str}";
        }

		public static void ExportLevelData(string levelName)
		{
		}
	}

}