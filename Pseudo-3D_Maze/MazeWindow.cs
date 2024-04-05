using Microsoft.VisualBasic;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;

namespace Pseudo_3D_Maze
{
    public partial class MazeWindow : Form
    {
        private Size screenSize = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
        private Maze maze;
        private bool isGameEnd = false;
        private Player player;
        private Graphics graphic;
        private BufferedGraphics buffer;
        private bool isPlayerMoving = false;
        private bool isChangingPlayerGazeDirection = false;
        private Rectangle miniMapBorder;
        private int yMiniMapOffset;
        private int xMiniMapOffset;
        private int miniMapElementHeight;
        private int miniMapElementWidth;
        private Pen miniMapBorderColor = new Pen(Color.Gold, 4);
        private Point [] trianglePoints = new Point[3];
        private VisiblePartOfMaze visiblePartOfMaze;


        public MazeWindow()
        {
            InitializeComponent();
            graphic = CreateGraphics();
            var miniMapSize = 450;
            buffer = BufferedGraphicsManager.Current.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            bool isCorrectInput = false;
            int mazeHeight = 0;
            while (!isCorrectInput)
            {
                if (int.TryParse(Interaction.InputBox($"Введите длину лабиринта, которая не должна превышать {miniMapSize / 2}:", "Окно ввода длиный лабиринта"), out mazeHeight))
                {
                    if (mazeHeight <= miniMapSize / 2)
                    {
                        isCorrectInput = true;
                    }
                }
            }
            isCorrectInput = false;
            int mazeWidth = 0;
            while (!isCorrectInput)
            {
                if (int.TryParse(Interaction.InputBox($"Введите ширину лабиринта, которая не должна превышать {miniMapSize / 2}:", "Окно ввода ширины лабиринта"), out mazeWidth))
                {
                    if (mazeWidth <= miniMapSize / 2)
                    {
                        isCorrectInput = true;
                    }
                }
            }
            yMiniMapOffset = miniMapSize % mazeHeight;
            xMiniMapOffset = miniMapSize % mazeWidth;
            miniMapElementHeight = miniMapSize / mazeHeight; //- 1;
            miniMapElementWidth = miniMapSize / mazeWidth; //- 1;
            maze = new Maze(mazeHeight, mazeWidth);
            maze.FillMaze();
            player = new Player(maze);
            miniMapBorder = new Rectangle(0, 0, miniMapSize, miniMapSize);
            player.GazeDirection = maze.GetStartPlayerGazeDirection();
            visiblePartOfMaze = new VisiblePartOfMaze(4, 3);
            //Cursor.Hide();

            FrameTimer.Start();
        }

        private void FrameTimerTick(object sender, EventArgs e)
        {
            buffer.Graphics.Clear(Color.DarkBlue);

            DrawMiniMap();
            buffer.Render();
            if (!isGameEnd)
            {
                if (player.Position == maze.GetPositionOfFinish())
                {
                    isGameEnd = true;
                }
                //if (isPlayerMoving || isChangingPlayerGazeDirection)
                //{
                //    //перерисовка карты и лабиринта
                //    //проверка закончил ли игрок игру
                //}
            }
            else
            {
                FrameTimer.Stop();
                MessageBox.Show("Вы выбрались из лабиринта!",
                    "Победное окно",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Application.Exit();
            }
        }

        private void MazeWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (!isGameEnd)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        PlayerMoveForward();
                        visiblePartOfMaze.ChangeVisiblePartOfMaze(maze, player);
                        break;

                    case Keys.Down:
                        PlayerMoveBackward();
                        visiblePartOfMaze.ChangeVisiblePartOfMaze(maze, player);
                        break;

                    case Keys.Left:
                        player.LeftTurn();
                        visiblePartOfMaze.ChangeVisiblePartOfMaze(maze, player);
                        break;

                    case Keys.Right:
                        player.RightTurn();
                        visiblePartOfMaze.ChangeVisiblePartOfMaze(maze, player);
                        break;
                }
            }
        }

        private void PlayerMoveForward()
        {
            if ((player.GazeDirection == Dir.Left) && (!isWall(player.Position.X + 1, player.Position.Y)))
            {
                maze.SetPlayerPosition(player.Position.X + 1, player.Position.Y);
                player.Position.X++;
            }
            else if(player.GazeDirection == Dir.Right && (!isWall(player.Position.X - 1, player.Position.Y)))
            {
                maze.SetPlayerPosition(player.Position.X - 1, player.Position.Y);
                player.Position.X--;
            }
            else if(player.GazeDirection == Dir.Down && (!isWall(player.Position.X, player.Position.Y + 1)))
            {
                maze.SetPlayerPosition(player.Position.X, player.Position.Y + 1);
                player.Position.Y++;
            }
            else if(player.GazeDirection == Dir.Up && (!isWall(player.Position.X, player.Position.Y - 1)))
            {
                maze.SetPlayerPosition(player.Position.X, player.Position.Y - 1);
                player.Position.Y--;
            }
        }

        private void PlayerMoveBackward()
        {
            if ((player.GazeDirection == Dir.Left) && (!isWall(player.Position.X - 1, player.Position.Y)))
            {
                maze.SetPlayerPosition(player.Position.X - 1, player.Position.Y);
                player.Position.X--;
            }
            else if (player.GazeDirection == Dir.Right && (!isWall(player.Position.X + 1, player.Position.Y)))
            {
                maze.SetPlayerPosition(player.Position.X + 1, player.Position.Y);
                player.Position.X++;
            }
            else if (player.GazeDirection == Dir.Down && (!isWall(player.Position.X, player.Position.Y - 1)))
            {
                maze.SetPlayerPosition(player.Position.X, player.Position.Y - 1);
                player.Position.Y--;
            }
            else if (player.GazeDirection == Dir.Up && (!isWall(player.Position.X, player.Position.Y + 1)))
            {
                maze.SetPlayerPosition(player.Position.X, player.Position.Y + 1);
                player.Position.Y++;
            }
        }
        private bool isWall(int x, int y)
        {
            if ((maze.GetMazeCell( x, y) == Items.Wall) 
                || (maze.GetMazeCell(x, y) == Items.SideStrongWall)
                || (maze.GetMazeCell(x, y) == Items.TopStrongWall)
                || (maze.GetMazeCell(x, y) == Items.BottomStrongWall))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DrawMiniMap()
        {
            buffer.Graphics.FillRectangle(Brushes.Black, miniMapBorder);
            buffer.Graphics.DrawRectangle(miniMapBorderColor, miniMapBorder.Location.X - 1, miniMapBorder.Location.Y - 1, miniMapBorder.Width + 1, miniMapBorder.Height + 1);
            for (int indexRowOfMiniMapElement = -1, mazeRow = 1; indexRowOfMiniMapElement < maze.Height; indexRowOfMiniMapElement++, mazeRow++)
            {
                if (mazeRow < maze.Height - 1)
                {
                    for (int indexColOfMiniMapElement = -1, mazeCol = 1; indexColOfMiniMapElement < maze.Width; indexColOfMiniMapElement++, mazeCol++)
                    {
                        if (mazeCol < maze.Width - 1)
                        {
                            DrawMiniMapElement(maze.GetMazeCell(mazeCol, mazeRow), (indexColOfMiniMapElement * miniMapElementWidth) - (xMiniMapOffset / 2), (indexRowOfMiniMapElement * miniMapElementHeight) - (yMiniMapOffset / 2));
                        }
                    }
                }
            }
        }

        private void DrawMiniMapElement(Items cell, int x, int y)
        {
            if (cell == Items.Wall)
            {
                buffer.Graphics.FillRectangle(Brushes.Orange, x, y, miniMapElementWidth, miniMapElementHeight);
            }
            else if (cell == Items.Road)
            {

            }
            else if (cell == Items.Player)
            {
                trianglePoints[0].X = x;
                trianglePoints[0].Y = y;
                trianglePoints[1].X = x + miniMapElementWidth;
                trianglePoints[1].Y = y;
                trianglePoints[2].X = x + miniMapElementWidth / 2;
                trianglePoints[2].Y = y + miniMapElementHeight;
                buffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                buffer.Graphics.FillPolygon(Brushes.Yellow, trianglePoints);
                buffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            }
            else if (cell == Items.TopStrongWall)
            {
                //buffer.Graphics.FillRectangle(Brushes.Coral, x, y, miniMapElementWidth, miniMapElementHeight);
            }
            else if (cell == Items.BottomStrongWall)
            {
                //buffer.Graphics.FillRectangle(Brushes.Coral, x, y, miniMapElementWidth, miniMapElementHeight);
            }
            else if (cell == Items.Finish)
            {
                x = x - miniMapElementWidth;
                if (miniMapElementHeight > miniMapElementWidth)
                {
                    buffer.Graphics.FillEllipse(Brushes.Red, x, y, miniMapElementWidth, miniMapElementWidth);
                }
                else
                {
                    buffer.Graphics.FillEllipse(Brushes.Red, x, y, miniMapElementHeight, miniMapElementHeight);
                }
            }
            else
            {
                //buffer.Graphics.FillRectangle(Brushes.Coral, x, y, miniMapElementWidth, miniMapElementHeight);
            }
        }

        private void DrawVisiblePartOfMaze()
        {
            for (int row = visiblePartOfMaze.size.Height - 1; row >= 0; row--)
            {
                for (int col = 0; col < visiblePartOfMaze.size.Width; col++)
                {
                    if ((visiblePartOfMaze.visiblePartOfMaze[row, col] == (int)Items.Wall)
                    || (visiblePartOfMaze.visiblePartOfMaze[row, col] == (int)Items.SideStrongWall)
                    || (visiblePartOfMaze.visiblePartOfMaze[row, col] == (int)Items.TopStrongWall)
                    || (visiblePartOfMaze.visiblePartOfMaze[row, col] == (int)Items.BottomStrongWall))
                    {
                        switch (col)
                        {

                        }

                    }
                }
            }
        }

        private void DrawWall(Point[] trapezoidPoints)
        {

            // //int playerRow, int playerCol)
            //// Позиция текущего элемента относительно игрока
            //int offsetX = col - player.Position.X;
            //int offsetY = row - playerRow;

            //// Предположим, что cellSize - это размер клетки, и playerX, playerY - позиция игрока на экране
            //int x = playerX + offsetX * cellSize;
            //int y = playerY + offsetY * cellSize;

            //// Проверяем, что текущая клетка находится в пределах видимой области экрана
            //if (x >= 0 && x < screenWidth && y >= 0 && y < screenHeight)
            //{
            //    // В зависимости от типа элемента рисуем соответствующую текстуру или просто закрашиваем цветом
            //    switch (visiblePartOfMaze.cells[row, col])
            //    {
            //        case CellType.Wall:
            //            // Отрисовка стены с учетом перспективы
            //            DrawPerspectiveWall(x, y);
            //            break;
            //        case CellType.Road:
            //            // Отрисовка дороги
            //            DrawRoad(x, y);
            //            break;
            //        // Добавьте обработку других типов элементов, если это необходимо
            //        default:
            //            // По умолчанию закрашиваем какой-то цветом
            //            DrawDefault(x, y);
            //            break;
            //    }
            //}

            //private void DrawPerspectiveWall(int x, int y)
            //{
            //    // Логика отрисовки стены с учетом перспективы
            //}

            //private void DrawRoad(int x, int y)
            //{
            //    // Логика отрисовки дороги
            //}

            //private void DrawDefault(int x, int y)
            //{
            //    // Логика отрисовки для остальных типов элементов
            //}

        }

        private void DrawRectangle(Point[] rectanglePoints)
        {

        }
    }
}
