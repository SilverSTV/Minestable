namespace Game.Scripts.Gameplay
{
    public struct CellChange
    {
        public int x, y;
        public CellState state;

        public CellChange(int x, int y, CellState state)
        {
            this.x = x;
            this.y = y;
            this.state = state;
        }
    }
}
