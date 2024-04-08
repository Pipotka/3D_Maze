using Microsoft.VisualBasic;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pseudo_3D_Maze
{
    public partial class MazeWindow : Form
    {
        private Size screenSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        private Maze maze;
        private bool isGameEnd = false;
        private Player player;
        private BufferedGraphics buffer;
        private Rectangle miniMapBorder;
        private int miniMapElementHeight;
        private int miniMapElementWidth;
        private bool isDifficultMap;
        private bool isPlayerActing = true;
        private Pen miniMapBorderColor = new Pen(Color.Gold, 4);
        private Point [] trianglePoints = new Point[3];
        private SolidBrush[] brushes; 
        private DateTime tp1;
        private DateTime tp2;
        TimeSpan elapsedTime;


        public MazeWindow()
        {
            InitializeComponent();
            buffer = BufferedGraphicsManager.Current.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            var isCorrectInput = false;
            var mazeHeight = 0;
            while (!isCorrectInput)
            {
                if (int.TryParse(Interaction.InputBox("Введите длину лабиринта:", "Окно ввода длиный лабиринта"), out mazeHeight))
                {
                        isCorrectInput = true;
                }
            }
            isCorrectInput = false;
            var mazeWidth = 0;
            while (!isCorrectInput)
            {
                if (int.TryParse(Interaction.InputBox("Введите ширину лабиринта:", "Окно ввода ширины лабиринта"), out mazeWidth))
                {
                        isCorrectInput = true;
                }
            }
            Size miniMapSize = new Size(450, 450);
            var yMiniMapOffset = miniMapSize.Height % mazeHeight;
            var xMiniMapOffset = miniMapSize.Width % mazeWidth;
            if ((mazeHeight == mazeWidth) && ((yMiniMapOffset == 0) && (xMiniMapOffset == 0)))
            {
                isDifficultMap = false;
                miniMapElementHeight = miniMapSize.Height / mazeHeight;
                miniMapElementWidth = miniMapSize.Width / mazeWidth;
            }
            else
            {
                isDifficultMap = true;
                miniMapElementHeight = miniMapSize.Height / 10;
                miniMapElementWidth = miniMapSize.Width / 10;
            }
            maze = new Maze(mazeHeight, mazeWidth);
            maze.FillMaze();
            player = new Player(maze);
            miniMapBorder = new Rectangle(0, 0, miniMapSize.Width, miniMapSize.Height);
            tp1 = DateTime.Now;
            tp2 = DateTime.Now;
            brushes = new SolidBrush [5];
            brushes[0] = new SolidBrush (Color.White);
            brushes[1] = new SolidBrush (Color.LightGray);
            brushes[2] = new SolidBrush(Color.Gray);
            brushes[3] = new SolidBrush(Color.DarkGray);
            brushes[4] = new SolidBrush(Color.DarkGray);
            Cursor.Hide();
            FrameTimer.Start();
        }

        private void FrameTimerTick(object sender, EventArgs e)
        {
            tp2 = DateTime.Now;
            elapsedTime = tp2 - tp1;
            tp1 = tp2;
            if (isPlayerActing)
            {
                buffer.Graphics.Clear(Color.Black);
                DrawVisiblePartOfMaze();
                DrawMiniMap();
                buffer.Render();
                isPlayerActing = false;
            }
            if (isGameEnd)
            {
                Cursor.Show();
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
                var ElapsedTime = (float)elapsedTime.TotalSeconds;
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        MoveForward(ElapsedTime);
                        isPlayerActing = true;
                        if ((maze.GetPositionOfFinish().X == (int)player.X) && (maze.GetPositionOfFinish().Y == (int)player.Y))
                        {
                            isGameEnd = true;
                        }
                        break;

                    case Keys.Down:
                        MoveBackward(ElapsedTime);
                        isPlayerActing = true;
                        if ((maze.GetPositionOfFinish().X == (int)player.X) && (maze.GetPositionOfFinish().Y == (int)player.Y))
                        {
                            isGameEnd = true;
                        }
                        break;

                    case Keys.Left:
                        LeftTurn(ElapsedTime);
                        isPlayerActing = true;
                        break;

                    case Keys.Right:
                        RightTurn(ElapsedTime);
                        isPlayerActing = true;
                        break;
                }
            }
        }

        private void MoveForward(float elapsedTime)
        {
            player.X += MathF.Sin(player.A) * player.Speed * elapsedTime;
            player.Y += MathF.Cos(player.A) * player.Speed * elapsedTime;
            if (isWall((int)player.X, (int)player.Y))
            {
                player.X -= MathF.Sin(player.A) * player.Speed * elapsedTime;
                player.Y -= MathF.Cos(player.A) * player.Speed * elapsedTime;
            }
            maze.SetPlayerPosition((int)player.X, (int)player.Y);
        }

        private void MoveBackward(float elapsedTime)
        {
            player.X -= MathF.Sin(player.A) * player.Speed * elapsedTime;
            player.Y -= MathF.Cos(player.A) * player.Speed * elapsedTime;
            if (isWall((int)player.X, (int)player.Y))
            {
                player.X += MathF.Sin(player.A) * player.Speed * elapsedTime;
                player.Y += MathF.Cos(player.A) * player.Speed * elapsedTime;
            }
            maze.SetPlayerPosition((int)player.X, (int)player.Y);
        }

        private void LeftTurn(float elapsedTime)
        {
            player.A -= (player.Speed * 0.75f) * elapsedTime;
        }

        private void RightTurn(float elapsedTime)
        {
            player.A += (player.Speed * 0.75f) * elapsedTime;
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
            if (!isDifficultMap)
            {
                for (int indexRowOfMiniMapElement = 0, mazeRow = 2; mazeRow < maze.Height - 1; indexRowOfMiniMapElement++, mazeRow++)
                {
                    for (int indexColOfMiniMapElement = 0, mazeCol = 2; mazeCol < maze.Width - 1; indexColOfMiniMapElement++, mazeCol++)
                    {
                        DrawMiniMapElement(maze.GetMazeCell(mazeCol, mazeRow), indexColOfMiniMapElement * miniMapElementWidth, indexRowOfMiniMapElement * miniMapElementHeight);
                    }
                }
            }
            else
            {
                for (int indexRowOfMiniMapElement = 0, mazeRowRelativeToPlayer = - 4; mazeRowRelativeToPlayer <= 5; indexRowOfMiniMapElement++, mazeRowRelativeToPlayer++)
                {
                    if (((int)player.Y + mazeRowRelativeToPlayer >= 1) && ((int)player.Y + mazeRowRelativeToPlayer < maze.Height - 1))
                    {
                        for (int indexColOfMiniMapElement = 0, mazeColRelativeToPlayer = -4; mazeColRelativeToPlayer <= 5; mazeColRelativeToPlayer++, indexColOfMiniMapElement++)
                        {
                            if (((int)player.X + mazeColRelativeToPlayer >= 1) && ((int)player.X + mazeColRelativeToPlayer < maze.Width - 1))
                            {
                                DrawMiniMapElement(maze.GetMazeCell((int)player.X + mazeColRelativeToPlayer, (int)player.Y + mazeRowRelativeToPlayer), indexColOfMiniMapElement * miniMapElementWidth, indexRowOfMiniMapElement * miniMapElementHeight);
                            }
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
            else if ((cell == Items.TopStrongWall) && (isDifficultMap))
            {
                buffer.Graphics.FillRectangle(Brushes.Orange, x, y, miniMapElementWidth, miniMapElementHeight);
            }
            else if ((cell == Items.BottomStrongWall)&& (isDifficultMap))
            {
                buffer.Graphics.FillRectangle(Brushes.Orange, x, y, miniMapElementWidth, miniMapElementHeight);
            }
            else if (cell == Items.Finish)
            {
                if (!isDifficultMap)
                {
                    x = x - miniMapElementWidth;
                }
                if (miniMapElementHeight > miniMapElementWidth)
                {
                    buffer.Graphics.FillEllipse(Brushes.Red, x, y, miniMapElementWidth, miniMapElementWidth);
                }
                else
                {
                    buffer.Graphics.FillEllipse(Brushes.Red, x, y, miniMapElementHeight, miniMapElementHeight);
                }
            }
            else if ((cell == Items.SideStrongWall) && (isDifficultMap))
            {
                buffer.Graphics.FillRectangle(Brushes.Orange, x, y, miniMapElementWidth, miniMapElementHeight);
            }
        }

        private void DrawVisiblePartOfMaze()
        {
           for (int xPositionOnScreen = 0; xPositionOnScreen < screenSize.Width; xPositionOnScreen++)
           {
                float rayAngle = (player.A - player.ViewingAngle / 2.0f) + ((float)xPositionOnScreen / (float)screenSize.Width) * player.ViewingAngle;
                var distanceToWall = 0.0f;
                var hitWall = false;
                var boundary = false;
                var eyeX = MathF.Sin(rayAngle);
                var eyeY = MathF.Cos(rayAngle);
                while (!hitWall && distanceToWall < player.ViewingDistance)
                {
                    distanceToWall += 0.1f;
                    var testX = (int)(player.X + eyeX * distanceToWall);
                    var testY = (int)(player.Y + eyeY * distanceToWall);
                    if (testX < 0 || testX >= maze.Width ||  testY < 0 || testY >= maze.Height)
                    {
                        hitWall = true;
                        distanceToWall = player.ViewingDistance;
                    }
                    else
                    {
                        if (isWall(testX, testY))
                        {
                            hitWall = true;
                            List<(float, float)> p = new List<(float, float)>();
                            for (int tx = 0; tx < 2; tx++)
                            {
                                for (int ty = 0; ty < 2; ty++)
                                {
                                    float vx = testX + tx - player.X;
                                    float vy = testY + ty - player.Y;
                                    var d = MathF.Sqrt(vx * vx + vy * vy);
                                    float dot = (eyeX * vx / d) + (eyeY * vy / d);
                                    p.Add((d, dot));
                                }
                            }
                            p.Sort((left, right) => left.Item1.CompareTo(right.Item1));
                            var bound = 0.005f;
                            if (Math.Acos(p[0].Item2) < bound || Math.Acos(p[1].Item2) < bound)
                            {
                                boundary = true;
                            }
                        }
                    }
                }
                var ceiling = (int)((float)(screenSize.Height / 2.0) - screenSize.Height / ((float)distanceToWall));
                var floor = screenSize.Height - ceiling;
                var drawingHeight = floor - ceiling;
                int shade;
                if (distanceToWall <= player.ViewingDistance / 6.0f)
                {
                    shade = 0; // first element of the brushes array
                }
                else if (distanceToWall < player.ViewingDistance / 5.0f)
                {
                    shade = 1; // second element of the brushes array
                }
                else if (distanceToWall < player.ViewingDistance / 3.0f)
                {
                    shade = 2; // third element of the brushes array
                }
                else if (distanceToWall < player.ViewingDistance)
                {
                    shade = 3; // fourth element of the brushes array
                }
                else
                {
                    shade = brushes.Length; // the wall will not be rendered
                }
                if (boundary)
                {
                    shade = 4;// The fourth element of the brushes array is the color of the edge
                }
                if (shade != brushes.Length)
                {
                    buffer.Graphics.FillRectangle(brushes[shade], xPositionOnScreen, ceiling, 1, drawingHeight);
                }
            }
        }
    }
}
