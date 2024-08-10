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
        public Action<int> PlayerStart;
        public Action<int> PlayerEnd;
        public Action<int> TurnStart;
        public Action<int> TurnEnd;

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

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && !eventKey.Pressed && eventKey.Keycode == Key.M)
            {
                NextPlayer();
            }
        }

        public Player GetPlayer(int num)
        {
            if (Players.Count < num || num < 0)
            {
                return null;
            }
            return Players[num];
        }

        /// <summary>
        /// 下一个Player行动 如果已没有Player 下一回合
        /// </summary>
        /// <returns></returns>
        public Player NextPlayer()
        {
            BattleManager.CellGridMgr.ToBlockInputState();
            PlayerEnd?.Invoke(currentPlayerIndex);

            var next = Players.FirstOrDefault(p => p.PlayerNumber > currentPlayerIndex && p.HasUnit());
            if (next is null)
            {
                currentPlayerIndex = 0;
                NextTurn();
            }
            else
            {
                currentPlayerIndex = next.PlayerNumber;
            }
            GD.Print($"{currentPlayerIndex} Player");
            PlayerStart?.Invoke(currentPlayerIndex);
            CurrentPlayer.Play();
            return CurrentPlayer;
        }

        /// <summary>
        /// 下一回合
        /// </summary>
        public void NextTurn()
        {
            TurnEnd?.Invoke(CurrentTurn);
            CurrentTurn++;
            if (CurrentTurn <= MaxTurn)
            {
                TurnStart?.Invoke(CurrentTurn);
            }
            else
            {

            }
        }

        /// <summary>
        /// 生成Player
        /// </summary>
        public void GeneratePlayers()
        {
            Players.Clear();
            Action<Player, int, PlayerAlignment> regist = (player, num, alignment) =>
            {
                Players.Add(player);
                player.PlayerNumber = num;
                player.Alignment = alignment;
                this.AddChild(player);
            };
            regist(new HumanPlayer(), 0, PlayerAlignment.Friend);   //玩家
            regist(new AiPlayer(), 1, PlayerAlignment.Friend);   //友军
            regist(new AiPlayer(), 2, PlayerAlignment.Enemy);  //敌人
            regist(new AiPlayer(), 3, PlayerAlignment.Third); //中立
            Players.Capacity = Players.Count;
        }

        public void RegisterUnit(Unit unit)
        {
            TurnStart += unit.OnTurnStart;
            TurnEnd += unit.OnTurnEnd;
        }

        public void UnRegisterUnit(Unit unit)
        {
            TurnStart -= unit.OnTurnStart;
            TurnEnd -= unit.OnTurnEnd;
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