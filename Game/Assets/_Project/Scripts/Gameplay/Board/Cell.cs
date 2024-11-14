namespace _Project.Scripts.Gameplay.Board
{
    public readonly struct Cell : ICell
    {
        public readonly CellView CellView;
        public char Symbol => CellView.Symbol.text[0];

        public Cell(CellView cellView) =>
            CellView = cellView;
    }
}