using Godot;
using SrpgFramework.CellGrid.CellGridState;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Level;
using SrpgFramework.Units;
using System.Collections.Generic;

namespace SrpgFramework.CellGrid
{
	public partial class CellGridManager : Node
    {
        public Dictionary<Vector2I, Cell> Cells { get; private set; } = new();

        public ICellGridState CurrentGridState { get; private set; }

        /// <summary>
        /// Default State As Block Input
        /// </summary>
        public ICellGridState BlockInputState { get; private set; }
        /// <summary>
        /// Default State As Idle
        /// </summary>
        public ICellGridState IdleState { get; private set; }
        /// <summary>
        /// Default State While an Unit is Selected
        /// </summary>
        public ICellGridState SelectUnitState { get; private set; }

        private Unit _currentUnit;
        public override void _Ready()
		{
            BlockInputState = new GridStateBlockInput();
            IdleState = new GridStateIdle(this);
            SelectUnitState = new GridStateSelectUnit(this);
            CurrentGridState = IdleState;
        }

        public bool ToState(ICellGridState nextState, Unit unit)
        {
            if (CurrentGridState.CanTranslateTo(nextState))
            {
                CurrentGridState?.Exit(_currentUnit);
                CurrentGridState = nextState;
                _currentUnit = unit;
                CurrentGridState.Enter(_currentUnit);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ToBlockInputState()
        {
            return ToState(BlockInputState, null);
        }

        public bool ToIdleState()
        {
            return ToState(IdleState, null);
        }
        public void RegisterCell(Cell cell)
        {
            if (Cells.TryAdd(cell.Coord, cell))
            {
                cell.OnCellClicked += OnCellClicked;
                cell.OnCellHighlighted += OnCellHighlighted;
                cell.OnCellDehighlighted += OnCellDehighlighted;
                cell.OnUnitClicked += OnUnitClicked;
                cell.OnUnitHighlighted += OnUnitHighlighted;
                cell.OnUnitDeHighlighted += OnUnitDehighlighted;
            }
        }

        public void UnRegisterCell(Cell cell)
        {
            if (Cells.Remove(cell.Coord))
            {
                cell.OnCellClicked -= OnCellClicked;
                cell.OnCellHighlighted -= OnCellHighlighted;
                cell.OnCellDehighlighted -= OnCellDehighlighted;
                cell.OnUnitClicked -= OnUnitClicked;
                cell.OnUnitHighlighted -= OnUnitHighlighted;
                cell.OnUnitDeHighlighted -= OnUnitDehighlighted;
            }
        }

        private void OnCellDehighlighted(Cell cell)
        {
            CurrentGridState.OnCellDehighlighted(_currentUnit, cell);
        }
        private void OnCellHighlighted(Cell cell)
        {
            CurrentGridState.OnCellHighlighted(_currentUnit, cell);
        }
        private void OnCellClicked(Cell cell)
        {
            CurrentGridState.OnCellClicked(_currentUnit, cell);
        }
        private void OnUnitClicked(Unit unit)
        {
            CurrentGridState.OnUnitClicked(_currentUnit, unit);
        }
        private void OnUnitHighlighted(Unit unit)
        {
            CurrentGridState.OnUnitHighlighted(_currentUnit, unit);
        }
        private void OnUnitDehighlighted(Unit unit)
        {
            CurrentGridState.OnUnitDehighlighted(_currentUnit, unit);
        }

        public void GenerateCellGrid(LevelData levelData)
        {
            var parent = new Node();
            parent.Name = "Cells";

            for(var i = 0; i < 20; i++)
            {
                for(var j = 0; j < 20; j++)
                {

                }
            }
        }
    }
}