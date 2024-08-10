using Godot;
using SrpgFramework.Units;

namespace SrpgFramework.Ai.Evaluators
{
    public abstract partial class Evaluator<T>: Resource
    {
        public string ID;
        [Export]
        public float Weight = 1;

        public abstract float Evaluate(T toEvaluate, Unit unit);
        public static string GetResourcePath(string evaluator)
        {
            return $"res://Resources/Evaluators/{evaluator}.tres";
        }
    }
}
