using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Tiles
{
    public enum TileType
    {
        GROUND = 1,
        SOLID_WALL = 2,
        EXIT_BOTH = 3,
        DOOR = 4,
        FIX_START = 5,
        BREAK_START = 6,
        FIX_BLOCK = 7,
        BREAK_BLOCK = 8,
        HOLE = 9,
        BUTTON = 10,
        BOTH_ACT_BLOCK = 11
    }
}
