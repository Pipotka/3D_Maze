using Pseudo_3D_Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pseudo_3D_Maze
{
    class Maze
    {
        private int[,] maze;
        private int noGoZone;
        private int mazeHeight;
        private int mazeWidth;

        public Maze(int mazeHeight, int mazeWidth)
        {
            maze = new int[mazeHeight, mazeWidth];
            noGoZone = 4;
            this.mazeHeight = mazeHeight;
            this.mazeWidth = mazeWidth;
        }

        public int GetMazeElement(int i, int j)
        {
            return maze[i, j];
        }

        public void FillMaze()
        {
            FillWalls();
            Random rand = new Random();
            int i = rand.Next(noGoZone / 2, mazeHeight - noGoZone / 2);
            int j = rand.Next(noGoZone / 2, mazeWidth - noGoZone / 2);
            List<Point> ListOfAvailableWalls = new List<Point>();

            maze[i, j] = 0;
            InList(ListOfAvailableWalls, i, j);

            while (ListOfAvailableWalls.Count != 0)
            {
                int RandWall;
                RandWall = rand.Next(0, ListOfAvailableWalls.Count);
                i = ListOfAvailableWalls[RandWall].Y;
                j = ListOfAvailableWalls[RandWall].X;

                if (CountOfRoads(i, j) == 1)
                {
                    maze[i, j] = 0;
                    InList(ListOfAvailableWalls, i, j);
                }

                ListOfAvailableWalls.RemoveAt(RandWall);
            }

            SetPlayer();
            SetFinish();
        }

        private void FillWalls()
        {
            for (int i = 0; i < mazeHeight; i++)
            {
                for (int j = 0; j < mazeWidth; j++)
                {
                    if (NotOutMaze(i, j))
                    {
                        maze[i, j] = 1;
                    }
                    else
                    {
                        maze[i, j] = 2;
                    }
                }
            }
        }

        private void InList(List<Point> list, int i, int j)
        {
            Point cell = new Point();
            for (int mod = 1; mod != 5; mod++)
            {
                if (AroundCell(i, j, Items.Wall, mod))
                {
                    SetPoint(ref cell, i, j, mod);
                    list.Add(cell);
                }
            }
        }

        private bool AroundCell(int i, int j, Items item, int mod)
        {
            switch (mod)
            {
                case 1:
                    if (NotOutMaze(i - 1, j))
                    {
                        if (maze[i - 1, j] == (int)item)
                        {
                            return true;
                        }
                    }
                    return false;

                case 2:
                    if (NotOutMaze(i, j - 1))
                    {
                        if (maze[i, j - 1] == (int)item)
                        {
                            return true;
                        }
                    }
                    return false;

                case 3:
                    if (NotOutMaze(i, j + 1))
                    {
                        if (maze[i, j + 1] == (int)item)
                        {
                            return true;
                        }
                    }
                    return false;

                case 4:
                    if (NotOutMaze(i + 1, j))
                    {
                        if (maze[i + 1, j] == (int)item)
                        {
                            return true;
                        }
                    }
                    return false;

                case 5:
                    if (NotOutMaze(i - 1, j - 1))
                    {
                        if (maze[i - 1, j - 1] == (int)item)
                        {
                            return true;
                        }
                    }
                    return false;

                case 6:
                    if (NotOutMaze(i - 1, j + 1))
                    {
                        if (maze[i - 1, j + 1] == (int)item)
                        {
                            return true;
                        }
                    }
                    return false;

                case 7:
                    if (NotOutMaze(i + 1, j - 1))
                    {
                        if (maze[i + 1, j - 1] == (int)item)
                        {
                            return true;
                        }
                    }
                    return false;

                case 8:
                    if (NotOutMaze(i + 1, j + 1))
                    {
                        if (maze[i + 1, j + 1] == (int)item)
                        {
                            return true;
                        }
                    }
                    return false;

                default:
                    return false;
            }
        }

        private bool NotOutMaze(int i, int j)
        {
            if (!((i < noGoZone / 2 || i >= mazeHeight - noGoZone / 2) || (j < noGoZone / 2 || j >= mazeWidth - noGoZone / 2)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetPoint(ref Point cell, int i, int j, int mod)
        {
            switch (mod)
            {
                case 1:
                    cell.X = j;
                    cell.Y = i - 1;
                    break;

                case 2:
                    cell.X = j - 1;
                    cell.Y = i;
                    break;

                case 3:
                    cell.X = j + 1;
                    cell.Y = i;
                    break;

                case 4:
                    cell.X = j;
                    cell.Y = i + 1;
                    break;

                case 5:
                    cell.X = j - 1;
                    cell.Y = i - 1;
                    break;

                case 6:
                    cell.X = j + 1;
                    cell.Y = i - 1;
                    break;

                case 7:
                    cell.X = j - 1;
                    cell.Y = i + 1;
                    break;

                case 8:
                    cell.X = j + 1;
                    cell.Y = i + 1;
                    break;
            }
        }

        private int CountOfRoads(int i, int j)
        {
            int count = 0;

            for (int mod = 1; mod != 5; mod++)
            {
                switch (mod)
                {
                    case 1:
                        if (NotOutMaze(i - 1, j))
                        {
                            if (maze[i - 1, j] == 0)
                            {
                                count++;
                            }
                        }
                        break;

                    case 2:
                        if (NotOutMaze(i, j - 1))
                        {
                            if (maze[i, j - 1] == 0)
                            {
                                count++;
                            }
                        }
                        break;

                    case 3:
                        if (NotOutMaze(i, j + 1))
                        {
                            if (maze[i, j + 1] == 0)
                            {
                                count++;
                            }
                        }
                        break;

                    case 4:
                        if (NotOutMaze(i + 1, j))
                        {
                            if (maze[i + 1, j] == 0)
                            {
                                count++;
                            }
                        }
                        break;

                    case 5:
                        if (NotOutMaze(i - 1, j - 1))
                        {
                            if (maze[i - 1, j - 1] == 1)
                            {
                                count++;
                            }
                        }
                        break;

                    case 6:
                        if (NotOutMaze(i - 1, j + 1))
                        {
                            if (maze[i - 1, j + 1] == 1)
                            {
                                count++;
                            }
                        }
                        break;

                    case 7:
                        if (NotOutMaze(i + 1, j - 1))
                        {
                            if (maze[i + 1, j - 1] == 1)
                            {
                                count++;
                            }
                        }
                        break;

                    case 8:
                        if (NotOutMaze(i + 1, j + 1))
                        {
                            if (maze[i + 1, j + 1] == 1)
                            {
                                count++;
                            }
                        }
                        break;
                }
            }
            return count;
        }

        private void SetPlayer()
        {
            int i = 2;
            int j = 2;

            if (maze[i, j] == 0)
            {
                maze[i, j] = (int)Items.Player;
            }
            else
            {
                i++;
                j++;
                for (int mod = 1; mod != 9; mod++)
                {
                    if (AroundCell(i, j, (int)Items.Road, mod))
                    {
                        SetItem(i, j, (int)Items.Player, mod);
                        break;
                    }
                }
            }
        }

        private void SetItem(int i, int j, int item, int mod)
        {
            switch (mod)
            {
                case 1:
                    maze[i - 1, j] = item;
                    break;

                case 2:
                    maze[i, j - 1] = item;
                    break;

                case 3:
                    maze[i, j + 1] = item;
                    break;

                case 4:
                    maze[i + 1, j] = item;
                    break;

                case 5:
                    maze[i - 1, j - 1] = item;
                    break;

                case 6:
                    maze[i - 1, j + 1] = item;
                    break;

                case 7:
                    maze[i + 1, j - 1] = item;
                    break;

                case 8:
                    maze[i + 1, j + 1] = item;
                    break;
            }
        }

        private void SetFinish()
        {
            int i = mazeHeight - 2;
            int j = mazeWidth - 2;

            for (; i > 1; i--)
            {
                if (maze[i, j - 1] == 0)
                {
                    maze[i, j] = (int)Items.Finish;
                    break;
                }
            }
        }

        public Point GetPlayerPosition()
        {
            Point currentPlayerPosition = new Point();
            bool isFoundPlayer = false;
            for (int row = noGoZone / 2; row < mazeHeight; row++)
            {
                if (!isFoundPlayer)
                {
                    for (int column = noGoZone / 2; column < mazeWidth; column++)
                    {
                        if (maze[row, column] == (int)Items.Player)
                        {
                            currentPlayerPosition.X = column;
                            currentPlayerPosition.Y = row;
                            isFoundPlayer = true;
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            return currentPlayerPosition;
        }

        public Dir GetStartPlayerGazeDirection()
        {
            Point playerPosition = GetPlayerPosition();
            Dir playergazeDirection = Dir.None;
            for (int mod = 1; mod <= 4;  mod++)
            {
                if (AroundCell(playerPosition.Y, playerPosition.X, Items.Road, mod))
                {
                    playergazeDirection = GazeDirection(mod);
                }
            }
            return playergazeDirection;
        }

        private Dir GazeDirection(int mod)
        {
            switch (mod)
            {
                case 1:
                    return Dir.Up;
                    break;

                case 2:
                    return Dir.Left;
                    break;

                case 3:
                    return Dir.Right;
                    break;

                case 4:
                    return Dir.Down;
                    break;
                default:
                    return Dir.None;
            }
        }
    }
}
