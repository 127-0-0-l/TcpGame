using System.Windows.Shapes;

namespace Client
{
    class Lines
    {
        public Rectangle Rct { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Lines(Rectangle rct, int i, int j)
        {
            Rct = rct;
            X = i;
            Y = j;
        }
    }
}
