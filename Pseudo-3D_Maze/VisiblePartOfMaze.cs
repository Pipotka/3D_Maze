using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pseudo_3D_Maze
{
    struct VisiblePartOfMaze
    {
        public int[,] visiblePartOfMaze;
        public Size size;

        public VisiblePartOfMaze(int height, int width)
        {
            visiblePartOfMaze = new int[height, width];
            size = new Size(width, height);
        }

        public void ChangeVisiblePartOfMaze(Maze maze, Player player)
        {
            if (player.GazeDirection == Dir.Down)
            {
                DownPart(maze, player.Position);
            }
            else if (player.GazeDirection == Dir.Left)
            {
                LeftPart(maze, player.Position);
            }
            else if (player.GazeDirection == Dir.Up)
            {
                UpPart(maze, player.Position);
            }
            else if (player.GazeDirection == Dir.Right)
            {
                RightPart(maze, player.Position);
            }
        }

        private void DownPart(Maze maze, Point playerPosition)
        {
            for (int row = 0; row < size.Height; row++)
            {
                for (int col = 0; col < size.Width; col++)
                {
                    if (((row >= 0) && (row < maze.Height)) && ((col >= 0) && (col < maze.Width)))
                    {
                        visiblePartOfMaze[row, col] = (int)maze.GetMazeCell(playerPosition.X - (size.Width / 2) + col, playerPosition.Y + row);//, playerPosition.X + (size.Width / 2) - col);
                    }
                    else
                    {
                        visiblePartOfMaze[row, col] = (int)Items.Road;
                    }
                }
            }
        }
        private void LeftPart(Maze maze, Point playerPosition)
        {
            for (int colOfMaze = 0; colOfMaze < size.Height; colOfMaze++)
            {
                for (int rowOfMaze = 0; rowOfMaze < size.Width; rowOfMaze++)
                {
                    if (((colOfMaze >= 0) && (colOfMaze < maze.Height)) && ((rowOfMaze >= 0) && (rowOfMaze < maze.Width)))
                    {
                        visiblePartOfMaze[colOfMaze, rowOfMaze] = (int)maze.GetMazeCell(playerPosition.Y + (size.Width / 2) - rowOfMaze, playerPosition.X + colOfMaze);
                    }
                    else
                    {
                        visiblePartOfMaze[colOfMaze, rowOfMaze] = (int)Items.Road;
                    }
                }
            }
        }
        private void RightPart(Maze maze, Point playerPosition)
        {
            for (int colOfMaze = 0; colOfMaze < size.Height; colOfMaze++)
            {
                for (int rowOfMaze = 0; rowOfMaze < size.Width; rowOfMaze++)
                {
                    if (((colOfMaze >= 0) && (colOfMaze < maze.Height)) && ((rowOfMaze >= 0) && (rowOfMaze < maze.Width)))
                    {
                        visiblePartOfMaze[colOfMaze, rowOfMaze] = (int)maze.GetMazeCell(playerPosition.Y - (size.Width / 2) + rowOfMaze, playerPosition.X - colOfMaze);
                    }
                    else
                    {
                        visiblePartOfMaze[colOfMaze, rowOfMaze] = (int)Items.Road;
                    }
                }
            }
        }
        private void UpPart(Maze maze, Point playerPosition)
        {
            for (int row = 0; row < size.Height; row++)
            {
                for (int col = 0; col < size.Width; col++)
                {
                    if (((row >= 0) && (row < maze.Height)) && ((col >= 0) && (col < maze.Width)))
                    {
                        visiblePartOfMaze[row, col] = (int)maze.GetMazeCell(playerPosition.X + (size.Width / 2) - col, playerPosition.Y - row);//, playerPosition.X - (size.Width / 2) + col);
                    }
                    else
                    {
                        visiblePartOfMaze[row, col] = (int)Items.Road;
                    }
                }
            }
        }
    }
}
