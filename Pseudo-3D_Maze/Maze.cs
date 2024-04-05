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
        public int Height { get; }
        public int Width { get; }
        private Point positionOfFinish;

        public Maze(int mazeHeight, int mazeWidth)
        {
            noGoZone = 2;
            maze = new int[mazeHeight + noGoZone * 2, mazeWidth + noGoZone * 2];
            this.Height = mazeHeight + noGoZone * 2;
            this.Width = mazeWidth + noGoZone * 2;
        }

        public Items GetMazeCell(int x, int y)
        {
            if (((x >= 0) && (x < Width))  && ((y >= 0) && (y < Height)))
            {
                return (Items)maze[y, x];
            }
            else
            {
                return Items.Void;
            }
        }

        public void FillMaze()
        {
            FillWalls();
            Random rand = new Random();
            int i = rand.Next(noGoZone, Height - noGoZone);
            int j = rand.Next(noGoZone, Width - noGoZone);
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
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (NotOutMaze(i, j))
                    {
                        maze[i, j] = (int)Items.Wall;
                    }
                    else if (((i == 1 || i == Height - noGoZone) || (j == 1 || j == Width - noGoZone)))
                    {
                        if ((i == 1) && ((j > 0) && (j < Width - 1)))
                        {
                            maze[i, j] = (int)Items.TopStrongWall;
                        }
                        else if ((i == Height - noGoZone) && ((j > 0) && (j < Width - 1)))
                        {
                            maze[i, j] = (int)Items.BottomStrongWall;
                        }
                        else
                        {
                            maze[i, j] = (int)Items.SideStrongWall;
                        }
                    }
                    else
                    {
                        maze[i, j] = (int)Items.SideStrongWall;
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

        public bool NotOutMaze(int i, int j)
        {
            if (!((i < noGoZone || i >= Height - noGoZone) || (j < noGoZone || j >= Width - noGoZone)))
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
            int i = Height - 2;
            int j = Width - 2;

            for (; i > 1; i--)
            {
                if (maze[i, j - 1] == 0)
                {
                    maze[i, j] = (int)Items.Finish;
                    positionOfFinish = new Point(j, i);
                    break;
                }
            }
        }

        public Point GetPlayerPosition()
        {
            Point currentPlayerPosition = new Point();
            bool isFoundPlayer = false;
            for (int row = noGoZone; row < Height; row++)
            {
                if (!isFoundPlayer)
                {
                    for (int column = noGoZone; column < Width; column++)
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

        public void SetPlayerPosition(int x, int y)
        {
            var playerCell = GetPlayerPosition();
            maze[playerCell.Y, playerCell.X] = (int)Items.Road;
            maze[y, x] = (int)Items.Player;
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

                case 2:
                    return Dir.Right;

                case 3:
                    return Dir.Left;

                case 4:
                    return Dir.Down;

                default:
                    return Dir.None;
            }
        }

        public Point GetPositionOfFinish()
        {
            return positionOfFinish;
        }
    }
}
