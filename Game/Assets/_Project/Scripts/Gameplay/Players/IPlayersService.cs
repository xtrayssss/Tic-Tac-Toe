using System.Collections;
using System.Threading;
using _Project.Scripts.Core.Services;
using _Project.Scripts.Gameplay.Board;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Gameplay.Players
{
    public interface IPlayersService : IService
    {
        public Player CurrentPlayer { get; set; }
        public BitArray PlayerBoard { get; }
        Player Player1 { get; set; }
        Player Player2 { get; set; }
        Board<Cell> Board { get;}
        public Player SwitchPlayer();
        public UniTask<int> PlayerMoveAsync(CancellationToken token);
        public void SetMove(int move);
        public Player CreatePlayer(PlayerType type, BoardGenerator boardGenerator);
        public void SwapSymbols();
    }
}