using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position()
    {

    }

    public Position(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public static Position operator +(Position p1, Position p2)
    {
        return new Position
        {
            X = p1.X + p2.X,
            Y = p1.Y + p2.Y
        };

    }
}

