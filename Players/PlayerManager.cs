using Godot;
using SrpgFramework.Global;
using SrpgFramework.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Players
{
	public partial class PlayerManager : Node
	{
        public Action<int> OnPlayerStart;
        public Action<int> OnPlayerEnd;
        public Action<int> OnTurnStart;
        public Action<int> OnTurnEnd;

        public List<Player> Players { get; private set; }
        public Player CurrentPlayer => Players[currentPlayerIndex];
        private int currentPlayerIndex;

        public int CurrentTurn { get; private set; }
        public int MaxTurn { get; private set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Players = new();
            var lvManager = BattleManager.LevelMgr;
            /*            
             lvManager.OnLevelStart += GameStart;
             lvManager.OnLevelEnd += GameEnd;
            */
            GeneratePlayers();
        }


        public Player GetPlayer(int num)
        {
            if (Players.Count < num || num < 0)
            {
                return null;
            }
            return Players[num];
        }

        public Player NextPlayer()
        {
            BattleManager.CellGridMgr.ToBlockInputState();
            OnPlayerEnd(currentPlayerIndex);

            var next = Players.First(p => p.PlayerNumber > currentPlayerIndex && p.HasUnit());
            if (next is null)
            {
                currentPlayerIndex = 0;
                NextTurn();
            }
            else
            {
                currentPlayerIndex = next.PlayerNumber;
            }
            OnPlayerStart(currentPlayerIndex);
            CurrentPlayer.Play();
            return CurrentPlayer;
        }

        public void NextTurn()
        {
            OnTurnEnd?.Invoke(CurrentTurn);
            CurrentTurn++;
            if (CurrentTurn <= MaxTurn)
            {
                OnTurnStart?.Invoke(CurrentTurn);
            }
            else
            {

            }
        }

        public void GeneratePlayers()
        {
            Players.Clear();
            Action<Player, int, PlayerAlignment> regist = (player, num, alignment) =>
            {
                Players.Add(player);
                player.PlayerNumber = num;
                player.Alignment = alignment;
            };
            regist(new HumanPlayer(), 0, PlayerAlignment.Friend);   //玩家
            regist(new AiPlayer(), 1, PlayerAlignment.Friend);   //友军
            regist(new AiPlayer(), 2, PlayerAlignment.Enemy);  //敌人
            regist(new AiPlayer(), 3, PlayerAlignment.Third); //中立
            Players.Capacity = Players.Count;
        }

        public void RegisterUnit(Unit unit)
        {
            OnTurnStart += unit.TurnStart;
            OnTurnEnd += unit.TurnEnd;
        }

        public void UnRegisterUnit(Unit unit)
        {
            OnTurnStart -= unit.TurnStart;
            OnTurnEnd -= unit.TurnEnd;
        }

        void GameStart()
        {
            currentPlayerIndex = -1;
            CurrentTurn = 0;
            NextPlayer();
        }

        public void GameEnd()
        {
        }
	}
}