using System.Threading;
using _Project.Scripts.Core.Services.Input;
using _Project.Scripts.Gameplay.Board;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;

namespace _Project.Scripts.Gameplay.Players.Human
{
    public class HumanPlayer : Player
    {
        private readonly IInputService _inputService;

        public HumanPlayer(
            PlayerConfig config,
            IInputService inputService,
            BoardGenerator boardGenerator) : base(config, boardGenerator)
        {
            _inputService = inputService;
        }

        public override async UniTask<int> GetMoveAsync(CancellationToken token)
        {
            int2 cellPosition = int.MaxValue;
         
            while (!BoardGenerator.Board.IsWithinBoard(cellPosition) || BoardGenerator.Board.IsOccupied(cellPosition))
            {
                float2 position = await _inputService.WaitForTapAsync(token);
                cellPosition = BoardGenerator.Board.GetCellPosition(position, BoardGenerator.Board.Container);
            }

            return BoardGenerator.Board.CoordsToBitIndex(cellPosition.x, cellPosition.y) + 1;
        }
    }
}