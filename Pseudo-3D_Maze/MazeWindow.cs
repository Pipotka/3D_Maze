using Microsoft.VisualBasic;
using System.Numerics;

namespace Pseudo_3D_Maze
{
    public partial class MazeWindow : Form
    {
        private Maze maze;
        private bool isGameEnd = false;
        private Player player;
        private Graphics miniMapGraphics;


        public MazeWindow()
        {
            InitializeComponent();
            miniMapGraphics = CreateGraphics();
            int noGoZone = 4;
            int mazeHeight = int.Parse(Interaction.InputBox("¬ведите длину лабиринта:", "ќкно ввода длиный лабиринта")) + noGoZone;
            int mazeWidth = int.Parse(Interaction.InputBox("¬ведите ширину лабиринта:", "ќкно ввода ширины лабиринта")) + noGoZone;
            maze = new Maze(mazeHeight, mazeWidth);
            maze.FillMaze();
            player = new Player(maze);
            Cursor.Hide();
            FrameTimer.Start();
        }

        private void FrameTimerTick(object sender, EventArgs e)
        {

        }

        private void MazeWindowKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    // движение игрока вперед
                    break;
                case Keys.Down:
                    // движение игрка назад
                    break;
                case Keys.Left:
                    player.LeftTurn();
                    break;
                case Keys.Right:
                    player.RightTurn();
                    break;
            }
        }

        
    }
}
