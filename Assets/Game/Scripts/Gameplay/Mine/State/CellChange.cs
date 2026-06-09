namespace Game.Scripts.Gameplay.Mine.State
{
    public struct CellChange
    {
        public int X, Y;
        public CellState State;

        public CellChange(int x, int y, CellState state)
        {
            X = x;
            Y = y;
            State = state;
        }
    }
}
