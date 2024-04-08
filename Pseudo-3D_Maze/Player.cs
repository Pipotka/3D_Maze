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
        public float X;
        public float Y;
        public float A;
        public float Speed;
        public float ViewingAngle; // FOV
        public float ViewingDistance; // Depth

        public Player(Maze maze)
        {
            Point Position = maze.GetPlayerPosition();
            A = 0.0f;
            X = (float)Position.X;
            Y = (float)Position.Y;
            ViewingAngle = 3.14159f / 3;
            ViewingDistance = 30.0f;
            Speed = 5.0f;
        }
    }
}
