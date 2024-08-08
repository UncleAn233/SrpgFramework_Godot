using Godot;
using System;
using System.Collections.Generic;

namespace SrpgFramework.Units
{
    public class UnitJsonData : IUnitData
    {
        public string Id { get; set; }
        public HashSet<string> Tags { get; set; }
        public int Lv { get; set; }
        public int Hp { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Mdef { get; set; }
        public int Dodge { get; set; }
        public int Mov { get; set; }
        public int Hate { get; set; }
    }
}