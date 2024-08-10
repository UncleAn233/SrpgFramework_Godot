using Godot;
using SrpgFramework.CellGrid.CellGridState;
using SrpgFramework.CellGrid.Cells;

namespace SrpgFramework.Units.Commands
{
    public abstract partial class Command :Resource, ICellGridState 
    {
        public virtual string CmdName { get; }

        public virtual void PreProcess(Unit self) { }
        public virtual void PostProcess(Unit self) { }

        public virtual void Process(Unit self, double delta) { }
        public virtual void PhysicsProcess(Unit self, double delta) { }
        
        public virtual void Enter(Unit self) { }
        public virtual void Exit(Unit self) { }
        public virtual void OnCellClicked(Unit self, Cell cell) { }
        public virtual void OnCellDehighlighted(Unit self, Cell cell) { }
        public virtual void OnCellHighlighted(Unit self, Cell cell) { }
        public virtual void OnUnitClicked(Unit self, Unit unit) { }
        public virtual void OnUnitDehighlighted(Unit self, Unit unit) { }
        public virtual void OnUnitHighlighted(Unit self, Unit unit) { }
        /// <summary>
        /// 指令是否可以执行
        /// </summary>
        public virtual bool CanAction(Unit self) { return false; }
        
        public bool ShouldExecute(Unit self) { return ShouldExecute(self, self.Cell); }
        /// <summary>
        /// 为否时 AI不会考虑执行该指令
        /// </summary>
        public virtual bool ShouldExecute(Unit unit, Cell cell) { return false; }
        /// <summary>
        /// 指令评分
        /// </summary>
        public virtual float Evaluate(Unit unit) { return -1; }
    }
}