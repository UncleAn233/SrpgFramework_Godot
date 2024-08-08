using Godot;
using SrpgFramework.Utilities;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace SrpgFramework.Level
{
	public class LevelData
	{
        public string Map { get; set; }
        public IList<UnitProfile> Units;
	
        public LevelData(string jsonStr)
        {
            var jo = JsonObject.Parse(jsonStr);
            Map = jo[nameof(Map)].GetValue<string>();

            Units = new List<UnitProfile>();
            foreach(var u in jo[nameof(Units)] as JsonArray)
            {
                Units.Add(new UnitProfile(u));
            }
        }

        public string ToJson()
        {
            return "";
        }
    }

    public struct UnitProfile
    {
        public Vector2I Cell;
        public string Id;
        public int Player;

        public UnitProfile(Vector2I vec,string id,int player)
        {
            Cell = vec;
            Id = id;
            Player = player;
        }

        public UnitProfile(JsonNode jsonNode)
        {
            Cell = MathUtility.StringToVector2I(jsonNode[nameof(Cell)].GetValue<string>());
            Id = jsonNode[nameof(Id)].GetValue<string>();
            Player = jsonNode[nameof(Player)].GetValue<int>();
        }
    }
}