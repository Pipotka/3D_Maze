using Pseudo_3D_Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseudo_3D_Maze
{
    struct Player
    {
        public Point Position;
        public Dir GazeDirection;

        public Player(Maze maze)
        {
            Position = new Point();
            Position = maze.GetPlayerPosition();
            GazeDirection = new Dir();
            GazeDirection = maze.GetStartPlayerGazeDirection();
        }

        public void LeftTurn()
        {
            GazeDirection++;
            if ((int)GazeDirection == 4)
            {
                GazeDirection = Dir.Left;
            }
        }

        public void RightTurn()
        {
            GazeDirection--;
            if ((int)GazeDirection == -1)
            {
                GazeDirection = Dir.Down;
            }
        }
    }
}
