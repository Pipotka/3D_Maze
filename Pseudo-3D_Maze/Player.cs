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
            if (GazeDirection == Dir.Down)
            {
                GazeDirection = Dir.Left;
            }
            else if(GazeDirection == Dir.Left)
            {
                GazeDirection = Dir.Up;
            }
            else if (GazeDirection == Dir.Up)
            {
                GazeDirection = Dir.Right;
            }
            else if(GazeDirection == Dir.Right)
            {
                GazeDirection = Dir.Down;
            }
        }

        public void RightTurn()
        {
            if (GazeDirection == Dir.Down)
            {
                GazeDirection = Dir.Right;
            }
            else if (GazeDirection == Dir.Right)
            {
                GazeDirection = Dir.Up;
            }
            else if (GazeDirection == Dir.Up)
            {
                GazeDirection = Dir.Left;
            }
            else if (GazeDirection == Dir.Left)
            {
                GazeDirection = Dir.Down;
            }
        }
    }
}
