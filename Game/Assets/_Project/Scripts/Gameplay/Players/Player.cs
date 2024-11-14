using System.Collections;
using System.Threading;
using _Project.Scripts.Gameplay.Board;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Gameplay.Players
{
    public abstract class Player
    {
        private const char EMPTY_SYMBOL = ' ';
        public BitArray Board { get; }
        public BoardGenerator BoardGenerator { get; }
        public SymbolSettings SymbolSettings;
        public string MoveText { get; }
        public string WinText { get; }

        protected Player(PlayerConfig config, BoardGenerator boardGenerator)
        {
            SymbolSettings = config.SymbolSettings;
            MoveText = config.MoveText;
            BoardGenerator = boardGenerator;
            Board = new BitArray(boardGenerator.Config.Size * boardGenerator.Config.Size);
            WinText = config.WinText;
        }

        public abstract UniTask<int> GetMoveAsync(CancellationToken token);

        public void SetMove(int move)
        {
            BoardGenerator.Board.Cells[move].CellView.PlayFlipInX();
            BoardGenerator.Board.Cells[move].CellView.SetSymbol(SymbolSettings.Symbol, SymbolSettings.Design);
            BoardGenerator.Board.SetMove(move, Board);
        }

        public void ResetMove(int move)
        {
            BoardGenerator.Board.Cells[move].CellView.SetSymbol(EMPTY_SYMBOL);
            BoardGenerator.Board.Cells[move].CellView.PlayFlipInY();
            BoardGenerator.Board.ResetMove(move, Board);
        }
    }
}