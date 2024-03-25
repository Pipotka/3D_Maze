using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseudo_3D_Maze
{
    enum Dir
    {
        // Нельзя менять последовательность,
        // иначе методы "RightTurn" и "LeftTurn" структуры Player будут работать не корректно
        Left,
        Up,
        Right,
        Down,
        None
    }
}
