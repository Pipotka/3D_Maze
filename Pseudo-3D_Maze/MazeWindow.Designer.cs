namespace Pseudo_3D_Maze
{
    partial class MazeWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            FrameTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // FrameTimer
            // 
            FrameTimer.Interval = 14;
            FrameTimer.Tick += FrameTimerTick;
            // 
            // MazeWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MazeWindow";
            WindowState = FormWindowState.Maximized;
            KeyDown += MazeWindowKeyDown;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer FrameTimer;
    }
}
