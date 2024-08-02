using SrpgFramework.Units;

namespace SrpgFramework.Ai.Evaluators
{
    public abstract class Evaluator<T>
    {
        public string ID;
        public float Weight = 1;

        public virtual void PreCalculate(Unit unit) { }
        public abstract float Evaluate(T toEvaluate, Unit unit);
    }
}
