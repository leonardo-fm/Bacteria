namespace Bacteria
{
    partial class MainWindow
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.Update = new System.Windows.Forms.Timer(this.components);
            this.btnReset = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.tbTimeSpeed = new System.Windows.Forms.TrackBar();
            this.lblBactLifOpt = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LifeForDuplication = new System.Windows.Forms.TextBox();
            this.LifeForIdle = new System.Windows.Forms.TextBox();
            this.MaxLife = new System.Windows.Forms.TextBox();
            this.MinLife = new System.Windows.Forms.TextBox();
            this.LifeCombo = new System.Windows.Forms.TextBox();
            this.DupChance = new System.Windows.Forms.TextBox();
            this.FerChanc = new System.Windows.Forms.TextBox();
            this.MinFertiliz = new System.Windows.Forms.TextBox();
            this.MaxFertiliz = new System.Windows.Forms.TextBox();
            this.FertilizationChance = new System.Windows.Forms.Label();
            this.MinFertilization = new System.Windows.Forms.Label();
            this.MaxFertilization = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.DupTimes = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.NumOfBacterias = new System.Windows.Forms.Label();
            this.FPS = new System.Windows.Forms.Label();
            this.LowGraph = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize) (this.Canvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.tbTimeSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.BackColor = System.Drawing.SystemColors.Desktop;
            this.Canvas.Location = new System.Drawing.Point(0, 0);
            this.Canvas.Margin = new System.Windows.Forms.Padding(0);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(510, 510);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            this.Canvas.Click += new System.EventHandler(this.Canvas_Click);
            // 
            // Update
            // 
            this.Update.Interval = 10;
            this.Update.Tick += new System.EventHandler(this.Update_Tick);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(447, 681);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(50, 30);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(391, 681);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(50, 30);
            this.btnPause.TabIndex = 2;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // tbTimeSpeed
            // 
            this.tbTimeSpeed.AccessibleName = "Time";
            this.tbTimeSpeed.LargeChange = 10;
            this.tbTimeSpeed.Location = new System.Drawing.Point(279, 681);
            this.tbTimeSpeed.Minimum = 1;
            this.tbTimeSpeed.Name = "tbTimeSpeed";
            this.tbTimeSpeed.Size = new System.Drawing.Size(106, 45);
            this.tbTimeSpeed.SmallChange = 10;
            this.tbTimeSpeed.TabIndex = 3;
            this.tbTimeSpeed.Value = 1;
            this.tbTimeSpeed.ValueChanged += new System.EventHandler(this.tbTimeSpeed_ValueChanged);
            // 
            // lblBactLifOpt
            // 
            this.lblBactLifOpt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((int) (0)));
            this.lblBactLifOpt.Location = new System.Drawing.Point(12, 528);
            this.lblBactLifOpt.Name = "lblBactLifOpt";
            this.lblBactLifOpt.Size = new System.Drawing.Size(132, 20);
            this.lblBactLifOpt.TabIndex = 5;
            this.lblBactLifOpt.Text = "Bacteria life options";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((int) (0)));
            this.label1.Location = new System.Drawing.Point(302, 528);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Fertilization options";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 560);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Life for duplicate";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 580);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Life for idle";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 610);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Max life";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 630);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Min life";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 650);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "Life near combo";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 680);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 15);
            this.label7.TabIndex = 12;
            this.label7.Text = "Duplication chance";
            // 
            // LifeForDuplication
            // 
            this.LifeForDuplication.Location = new System.Drawing.Point(118, 555);
            this.LifeForDuplication.Name = "LifeForDuplication";
            this.LifeForDuplication.Size = new System.Drawing.Size(80, 20);
            this.LifeForDuplication.TabIndex = 13;
            this.LifeForDuplication.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LifeForDuplication_KeyDown);
            // 
            // LifeForIdle
            // 
            this.LifeForIdle.Location = new System.Drawing.Point(118, 575);
            this.LifeForIdle.Name = "LifeForIdle";
            this.LifeForIdle.Size = new System.Drawing.Size(80, 20);
            this.LifeForIdle.TabIndex = 14;
            this.LifeForIdle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LifeForIdle_KeyDown);
            // 
            // MaxLife
            // 
            this.MaxLife.Location = new System.Drawing.Point(118, 605);
            this.MaxLife.Name = "MaxLife";
            this.MaxLife.Size = new System.Drawing.Size(80, 20);
            this.MaxLife.TabIndex = 15;
            this.MaxLife.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MaxLife_KeyDown);
            // 
            // MinLife
            // 
            this.MinLife.Location = new System.Drawing.Point(118, 625);
            this.MinLife.Name = "MinLife";
            this.MinLife.Size = new System.Drawing.Size(80, 20);
            this.MinLife.TabIndex = 16;
            this.MinLife.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MinLife_KeyDown);
            // 
            // LifeCombo
            // 
            this.LifeCombo.Location = new System.Drawing.Point(118, 645);
            this.LifeCombo.Name = "LifeCombo";
            this.LifeCombo.Size = new System.Drawing.Size(80, 20);
            this.LifeCombo.TabIndex = 17;
            this.LifeCombo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LifeCombo_KeyDown);
            // 
            // DupChance
            // 
            this.DupChance.Location = new System.Drawing.Point(118, 675);
            this.DupChance.Name = "DupChance";
            this.DupChance.Size = new System.Drawing.Size(80, 20);
            this.DupChance.TabIndex = 18;
            this.DupChance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DupChance_KeyDown);
            // 
            // FerChanc
            // 
            this.FerChanc.Location = new System.Drawing.Point(408, 595);
            this.FerChanc.Name = "FerChanc";
            this.FerChanc.Size = new System.Drawing.Size(80, 20);
            this.FerChanc.TabIndex = 24;
            this.FerChanc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FerChanc_KeyDown);
            // 
            // MinFertiliz
            // 
            this.MinFertiliz.Location = new System.Drawing.Point(408, 575);
            this.MinFertiliz.Name = "MinFertiliz";
            this.MinFertiliz.Size = new System.Drawing.Size(80, 20);
            this.MinFertiliz.TabIndex = 23;
            this.MinFertiliz.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MinFertiliz_KeyDown);
            // 
            // MaxFertiliz
            // 
            this.MaxFertiliz.Location = new System.Drawing.Point(408, 555);
            this.MaxFertiliz.Name = "MaxFertiliz";
            this.MaxFertiliz.Size = new System.Drawing.Size(80, 20);
            this.MaxFertiliz.TabIndex = 22;
            this.MaxFertiliz.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MaxFertiliz_KeyDown);
            // 
            // FertilizationChance
            // 
            this.FertilizationChance.Location = new System.Drawing.Point(300, 600);
            this.FertilizationChance.Name = "FertilizationChance";
            this.FertilizationChance.Size = new System.Drawing.Size(100, 15);
            this.FertilizationChance.TabIndex = 21;
            this.FertilizationChance.Text = "Fertilization chance";
            // 
            // MinFertilization
            // 
            this.MinFertilization.Location = new System.Drawing.Point(300, 580);
            this.MinFertilization.Name = "MinFertilization";
            this.MinFertilization.Size = new System.Drawing.Size(100, 15);
            this.MinFertilization.TabIndex = 20;
            this.MinFertilization.Text = "Min fertilization";
            // 
            // MaxFertilization
            // 
            this.MaxFertilization.Location = new System.Drawing.Point(300, 560);
            this.MaxFertilization.Name = "MaxFertilization";
            this.MaxFertilization.Size = new System.Drawing.Size(100, 15);
            this.MaxFertilization.TabIndex = 19;
            this.MaxFertilization.Text = "Max fertilization";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 700);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 15);
            this.label8.TabIndex = 25;
            this.label8.Text = "Duplication times";
            // 
            // DupTimes
            // 
            this.DupTimes.Location = new System.Drawing.Point(118, 695);
            this.DupTimes.Name = "DupTimes";
            this.DupTimes.Size = new System.Drawing.Size(80, 20);
            this.DupTimes.TabIndex = 26;
            this.DupTimes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DupTimes_KeyDown);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(391, 650);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 15);
            this.label9.TabIndex = 27;
            this.label9.Text = "Bacterias num.";
            // 
            // NumOfBacterias
            // 
            this.NumOfBacterias.Location = new System.Drawing.Point(470, 650);
            this.NumOfBacterias.Name = "NumOfBacterias";
            this.NumOfBacterias.Size = new System.Drawing.Size(46, 15);
            this.NumOfBacterias.TabIndex = 28;
            this.NumOfBacterias.Text = "99999";
            // 
            // FPS
            // 
            this.FPS.Location = new System.Drawing.Point(0, 0);
            this.FPS.Name = "FPS";
            this.FPS.Size = new System.Drawing.Size(27, 15);
            this.FPS.TabIndex = 30;
            this.FPS.Text = "9999";
            // 
            // LowGraph
            // 
            this.LowGraph.Location = new System.Drawing.Point(290, 643);
            this.LowGraph.Name = "LowGraph";
            this.LowGraph.Size = new System.Drawing.Size(93, 29);
            this.LowGraph.TabIndex = 29;
            this.LowGraph.Text = "Low graphics";
            this.LowGraph.UseVisualStyleBackColor = true;
            this.LowGraph.CheckedChanged += new System.EventHandler(this.LowGraph_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(509, 723);
            this.Controls.Add(this.LowGraph);
            this.Controls.Add(this.NumOfBacterias);
            this.Controls.Add(this.FPS);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.DupTimes);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.FerChanc);
            this.Controls.Add(this.MinFertiliz);
            this.Controls.Add(this.MaxFertiliz);
            this.Controls.Add(this.FertilizationChance);
            this.Controls.Add(this.MinFertilization);
            this.Controls.Add(this.MaxFertilization);
            this.Controls.Add(this.DupChance);
            this.Controls.Add(this.LifeCombo);
            this.Controls.Add(this.MinLife);
            this.Controls.Add(this.MaxLife);
            this.Controls.Add(this.LifeForIdle);
            this.Controls.Add(this.LifeForDuplication);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblBactLifOpt);
            this.Controls.Add(this.tbTimeSpeed);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.Canvas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize) (this.Canvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.tbTimeSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.CheckBox LowGraph;

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label NumOfBacterias;
        private System.Windows.Forms.Label FPS;

        private System.Windows.Forms.TextBox DupTimes;
        private System.Windows.Forms.Label label8;

        private System.Windows.Forms.Label MaxFertilization;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label FertilizationChance;
        private System.Windows.Forms.Label MinFertilization;
        private System.Windows.Forms.TextBox FerChanc;
        private System.Windows.Forms.TextBox MinFertiliz;
        private System.Windows.Forms.TextBox MaxFertiliz;
        private System.Windows.Forms.TextBox textBox3;

        private System.Windows.Forms.TextBox DupChance;
        private System.Windows.Forms.TextBox LifeCombo;
        private System.Windows.Forms.TextBox LifeForDuplication;
        private System.Windows.Forms.TextBox LifeForIdle;
        private System.Windows.Forms.TextBox MaxLife;
        private System.Windows.Forms.TextBox MinLife;

        private System.Windows.Forms.Label label7;

        private System.Windows.Forms.Label label6;

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.Label lblBactLifOpt;

        private System.Windows.Forms.Button btnPause;

        private System.Windows.Forms.Button btnReset;

        #endregion

        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.Timer Update;
        private System.Windows.Forms.TrackBar tbTimeSpeed;
    }
}