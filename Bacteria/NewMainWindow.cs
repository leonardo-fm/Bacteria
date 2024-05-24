using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bacteria
{
    public partial class NewMainWindow : Form
    {
        private bool gameIsStopped = false;

        private const int CANVAS_SIZE = 600;

        private int lifePointToDuplicate = 4;
        private int lifePointIdle = 2;

        private int minLife = 4;
        private int maxLife = 10;
        private double lifeNearestCombo = 0.5;

        private int duplicationChance = 100;
        
        [Description("How many time a bacteria can replicate itselfe in a single iteration")]
        private int timeOfDuplications = 1;

        private int maxTimeBeforeFertilization = 30;
        private int minTimeBeforeFertilization =  5;
        private int fertilizationChance = 40;
        
        private bool lowGraphics = false;
        private int bacteriasNumber => bacteriaSimulation.Count; 

        private readonly Color moldColor = Color.FromArgb(229, 245, 88);
        private readonly Color bgColor = Color.FromArgb(30, 30, 30);
        private readonly Color deadColorFrom = Color.FromArgb(100, 100, 100);
        private readonly Color deadColorTo = Color.FromArgb(30, 30, 30);

        private readonly Bitmap bitmap = new Bitmap(CANVAS_SIZE, CANVAS_SIZE);
        private readonly Random rnd = new Random();
        
        private int fps = 0;
        private int frames = 0;

        private Dictionary<(int x, int y), Tail> bacteriaSimulation = new Dictionary<(int x, int y), Tail>();
        private Dictionary<(int x, int y), Action> actionsToPerform = new Dictionary<(int x, int y), Action>();

        private static List<(int x, int y, Color color)> colorVector = new List<(int x, int y, Color color)>();
        
        private class Tail
        {
            private int value;

            public int Value
            {
                get => value;
                set
                {
                    this.value = value;
                    HasChanged = true;
                }
            }

            public bool HasChanged { get; private set; }

            public Tail(int value, bool hasChanged = true)
            {
                Value = value;
                HasChanged = hasChanged;
            }
        }

        private DrawManager drawManager;
        public NewMainWindow()
        {
            Text = "New main window";
            
            InitializeComponent();
            SetupOptionsValue();
            
            drawManager = new DrawManager(CANVAS_SIZE, CANVAS_SIZE, bgColor);
            Canvas.Image = drawManager.GetBitmap();
            drawManager.OnFrameDrawn += (from, frame) => Canvas.Image = frame;
            
            new Task(FPSCount).Start();
            Update.Start();
        }
        
        
        private void SetupOptionsValue()
        {
            LifeForDuplication.Text = lifePointToDuplicate.ToString();
            LifeForIdle.Text = lifePointIdle.ToString();
            MaxLife.Text = maxLife.ToString();
            MinLife.Text = minLife.ToString();
            LifeCombo.Text = lifeNearestCombo.ToString(CultureInfo.InvariantCulture);
            DupChance.Text = duplicationChance.ToString();
            DupTimes.Text = timeOfDuplications.ToString();
            
            MaxFertiliz.Text = maxTimeBeforeFertilization.ToString();
            MinFertiliz.Text = minTimeBeforeFertilization.ToString();
            FerChanc.Text = fertilizationChance.ToString();
            
            NumOfBacterias.Text = bacteriasNumber.ToString();
        }
        
        private void FPSCount()
        {
            while (true)
            {
                Thread.Sleep(1000);
                fps = frames;
                frames = 0;
            }
        }
        
        private Task drawTask;
        private void Update_Tick(object sender, EventArgs e)
        {
            RunSimulation();
            // List<(int x, int y, Color color)> newColorVector = GenerateColorVector();
            //
            //  if (drawTask != null && !drawTask.IsCompleted)
            //      drawTask.Wait();
            //  drawTask = new Task(() => drawManager.DrawFrame(colorVector));
            //  colorVector = newColorVector;
            //  drawTask.Start();
            
            RunModifications();
            
            frames++;
            FPS.Text = fps.ToString();
        }

        private void RunModifications()
        {
            foreach (((int x, int y), Action action) in actionsToPerform)
                action.Invoke();
            actionsToPerform.Clear();
        }

        private void RunSimulation()
        {
            foreach (((int x, int y) key, Tail tail) in bacteriaSimulation)
            {
                switch (tail.Value)
                {
                    case 0:
                        actionsToPerform.TryAdd((key.x, key.y), () => bacteriaSimulation.Remove((key.x, key.y)));
                        continue;
                    case < 0:
                        Fertilize(key.x, key.y);
                        continue;
                    case > 0:
                        TryDuplicate(key.x, key.y);
                        continue;
                }
            }
            
            NumOfBacterias.Text = bacteriasNumber.ToString();
        }
        
        private void Fertilize(int x, int y)
        {
            if (rnd.Next(0, 100) >= fertilizationChance) return;
            bacteriaSimulation[(x, y)].Value++;
        }

        private void TryDuplicate(int x, int y)
        {
            if (bacteriaSimulation[(x, y)].Value >= lifePointToDuplicate)
                Duplicate(x, y);
            else
                bacteriaSimulation[(x, y)].Value -= lifePointIdle;

            if (bacteriaSimulation[(x, y)].Value <= 0)
                Dead(x, y);
        }

        private void Duplicate(int x, int y)
        {
            bool hasDuplicated = false;
            List<(int, int)> nearestTiles = GetNearestTiles(x, y);
            int duplicationNumber = timeOfDuplications;

            int nearestTilesLength = nearestTiles.Count;
            int randomShiftIndex = rnd.Next(0, nearestTilesLength);
            
            for (int i = 0; i < nearestTiles.Count; i++)
            {
                (int x, int y) currentTile = nearestTiles[(i + randomShiftIndex) % nearestTilesLength];
                
                if (bacteriaSimulation.TryGetValue(currentTile, out Tail tail) && tail.Value != 0) continue;
                if (rnd.Next(100) >= duplicationChance) continue;

                double lifeBonus = 1;
                for (int j = 0; j < nearestTilesLength; j++)
                {
                    if (bacteriaSimulation.TryGetValue(currentTile, out Tail tail2) && tail2.Value > 0)
                        lifeBonus += lifeNearestCombo;
                }

                int tmpX = currentTile.x;
                int tmpY = currentTile.y;
                actionsToPerform.TryAdd((tmpX, tmpY), () =>
                {
                    bacteriaSimulation.TryAdd((tmpX, tmpY), new Tail((int)(rnd.Next(minLife, maxLife) * lifeBonus)));
                    bitmap.SetPixel(tmpX, tmpY, moldColor);
                });

                hasDuplicated = true;
                duplicationNumber -= 1;
                if (duplicationNumber == 0) break;
            }
            
            if (hasDuplicated)
                bacteriaSimulation[(x, y)].Value -= lifePointToDuplicate;
        }

        private List<(int, int)> GetNearestTiles(int x, int y)
        {
            List<(int, int)> nearTiles = new List<(int, int)>();
            
            if (y + 1 < CANVAS_SIZE)
                nearTiles.Add((x, y + 1));
            if (y - 1 > 0)
                nearTiles.Add((x, y - 1));
            if (x + 1 < CANVAS_SIZE)
                nearTiles.Add((x + 1, y));
            if (x - 1 > 0)
                nearTiles.Add((x - 1, y));

            return nearTiles;
        }

        private void Dead(int x, int y)
        {
            bacteriaSimulation[(x, y)].Value = -rnd.Next(minTimeBeforeFertilization, maxTimeBeforeFertilization);
        }

        private List<(int x, int y, Color color)> GenerateColorVector()
        {
            List<(int x, int y, Color color)> result = [];

            foreach (((int x, int y) key, Tail tail) in bacteriaSimulation)
            {
                if (!tail.HasChanged) continue;
                switch (tail.Value)
                {
                    case 0:
                        result.Add((key.x, key.y, bgColor));
                        continue;
                    case > 0:
                        result.Add((key.x, key.y, moldColor));
                        break;
                    default:
                    {
                        if (!lowGraphics)
                            result.Add((key.x, key.y, GetColorInterpolation(tail.Value)));
                        else
                            result.Add((key.x, key.y, bgColor));
                        break;
                    }
                }
            }
            
            return result;
        }
        
        private Color GetColorInterpolation(int levelOfFertilization)
        {
            return Color.FromArgb
            (
                GetProportion(-maxTimeBeforeFertilization, levelOfFertilization, deadColorFrom.R - deadColorTo.R) + deadColorTo.R,
                GetProportion(-maxTimeBeforeFertilization, levelOfFertilization, deadColorFrom.G - deadColorTo.G) + deadColorTo.G,
                GetProportion(-maxTimeBeforeFertilization, levelOfFertilization, deadColorFrom.B - deadColorTo.B) + deadColorTo.B
            );
        }

        private int GetProportion(double num1, double num2, double num3)
        {
            double result = num3 * (num2 / num1);
            return (int) result;
        }
        
        #region Comands

        private void Canvas_Click(object sender, EventArgs e)
        {
            int positionX = MousePosition.X - Left - 8;
            int positionY = MousePosition.Y - Top - 31;

            if (positionX >= 0 && positionX < CANVAS_SIZE && positionY >= 0 && positionY < CANVAS_SIZE)
                bacteriaSimulation.TryAdd((positionX, positionY), new Tail(rnd.Next(minLife, maxLife)));
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (gameIsStopped) return;

            Update.Stop();
            bacteriaSimulation.Clear();
            drawManager.ResetBitmap();
            Update.Start();

            Console.WriteLine("--- Reset ---");
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (!gameIsStopped)
            {
                Update.Stop();
                gameIsStopped = true;
                btnPause.Text = "Resume";
                Console.WriteLine("--- Pause ---");
            }
            else
            {
                Update.Start();
                gameIsStopped = false;
                btnPause.Text = "Pause";
                Console.WriteLine("--- Resume ---");
            }
        }

        private void tbTimeSpeed_ValueChanged(object sender, EventArgs e)
        {
            Update.Interval = tbTimeSpeed.Value * 10;
        }
        
        private void LowGraph_CheckedChanged(object sender, EventArgs e)
        {
            lowGraphics = LowGraph.Checked;
            if (lowGraphics)
                drawManager.ResetBitmap();
        }

        #endregion

        #region Bacteria options

        private void LifeForDuplication_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (int.TryParse(LifeForDuplication.Text, out int val))
                    lifePointToDuplicate = val;
        }

        private void LifeForIdle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (int.TryParse(LifeForIdle.Text, out int val))
                    lifePointIdle = val;
        }

        private void MaxLife_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)        
                if (int.TryParse(MaxLife.Text, out int val))
                    maxLife = val;
        }

        private void MinLife_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)     
                if (int.TryParse(MinLife.Text, out int val))
                    minLife = val;
        }

        private void LifeCombo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)     
                if (int.TryParse(LifeCombo.Text, out int val))
                    lifeNearestCombo = val;
        }

        private void DupChance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(DupChance.Text, out int val))
                    duplicationChance = val;
        }
        
        private void DupTimes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(DupTimes.Text, out int val))
                    timeOfDuplications = val;
        }

        #endregion

        #region Fertilization options

        private void MaxFertiliz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(MaxFertiliz.Text, out int val))
                    maxTimeBeforeFertilization = val;
        }

        private void MinFertiliz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(MinFertiliz.Text, out int val))
                    minTimeBeforeFertilization = val;
        }

        private void FerChanc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(FerChanc.Text, out int val))
                    fertilizationChance = val;
        }

        #endregion
    }
}