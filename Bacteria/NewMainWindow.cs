using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Bacteria
{
    public partial class NewMainWindow : Form
    {
        private bool gameIsStopped = false;

        private const int CANVAS_SIZE = 600;
        private const int CANVAS_SIZE_FOR_LOOP = 599;

        private int lifePointToDuplicate = 4;
        private int lifePointIdle = 2;

        private int minLife = 3;
        private int maxLife = 8;
        private double lifeNearestCombo = 0.5;

        private int duplicationChance = 40;
        private int timeOfDuplications = 1;

        private int maxTimeBeforeFertilization = 30;
        private int minTimeBeforeFertilization =  5;
        private int fertilizationChance = 40;
        
        private bool lowGraphics = false;

        private readonly Color moldColor = Color.FromArgb(229, 245, 88);
        private readonly Color bgColor = Color.FromArgb(30, 30, 30);
        private readonly Color deadColorFrom = Color.FromArgb(100, 100, 100);
        private readonly Color deadColorTo = Color.FromArgb(30, 30, 30);

        private readonly Bitmap bitmap = new Bitmap(CANVAS_SIZE, CANVAS_SIZE);
        private readonly Random rnd = new Random();

        private int[,] bacteriasStatusOne = new int[CANVAS_SIZE, CANVAS_SIZE];
        private int[,] bacteriasStatusTwo = new int[CANVAS_SIZE, CANVAS_SIZE];

        public NewMainWindow()
        {
            InitializeComponent();
            SetupBackGroundColor();
            SetupOptionsValue();
            Update.Start();
        }

        private void SetupBackGroundColor()
        {
            for (int x = 0; x < CANVAS_SIZE_FOR_LOOP; x++)
            {
                for (int y = 0; y < CANVAS_SIZE_FOR_LOOP; y++)
                {
                    bitmap.SetPixel(x, y, bgColor);
                }
            }

            Canvas.Image = bitmap;
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

        private void Update_Tick(object sender, EventArgs e)
        {
            EvolveBacteria();
        }

        private int bacteriasNumber = 0; 
        private void EvolveBacteria()
        {
            bacteriasNumber = 0;
            
            for (int x = 0; x < CANVAS_SIZE_FOR_LOOP; x++)
            {
                for (int y = 0; y < CANVAS_SIZE_FOR_LOOP; y++)
                {
                    if (bacteriasStatusOne[x, y] == 0) continue;

                    if (bacteriasStatusOne[x, y] < 0)
                    {
                        Fertilize(x, y);
                        continue;
                    }

                    Duplicate(x, y);
                }
            }

            Canvas.Image = bitmap;
            NumOfBacterias.Text = bacteriasNumber.ToString();
            
            bacteriasStatusOne = (int[,]) bacteriasStatusTwo.Clone();
            bacteriasStatusTwo = (int[,]) bacteriasStatusOne.Clone();
        }

        private void Fertilize(int x, int y)
        {
            intHolder = rnd.Next(0, 100) < fertilizationChance ? 1 : 0;
            bacteriasStatusTwo[x, y] += intHolder;

            if(!lowGraphics)
                bitmap.SetPixel(x, y, GetColorInterpolation(bacteriasStatusTwo[x, y]));
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
            var result = num3 * (num2 / num1);
            return (int) result;
        }

        private void Duplicate(int x, int y)
        {
            var nearestTiles = GetNearestTiles(x, y);
            ShuffleTiles(nearestTiles);
            var duplicationNumber = timeOfDuplications;

            for (var i = 0; i < nearestTiles.GetLength(0); i++)
            {
                if (nearestTiles[i, 0] != -1 && nearestTiles[i, 1] != -1)
                {
                    if (bacteriasStatusOne[nearestTiles[i, 0], nearestTiles[i, 1]] != 0) continue;
                    if (rnd.Next(100) >= duplicationChance) continue;

                    if(duplicationNumber == 0) continue;
                    duplicationNumber -= 1;
                    
                    double lifeBonus = 1;
                    for (int b = 0; b < nearestTiles.GetLength(0); b++)
                    {
                        if (nearestTiles[b, 0] != -1 && nearestTiles[b, 1] != -1)
                        {
                            if (bacteriasStatusOne[nearestTiles[b, 0], nearestTiles[b, 1]] > 0)
                                lifeBonus += lifeNearestCombo;
                        }
                    }

                    bacteriasStatusTwo[nearestTiles[i, 0], nearestTiles[i, 1]] =
                        (int) (rnd.Next(minLife, maxLife) * lifeBonus);

                    bacteriasNumber++;

                    bitmap.SetPixel(nearestTiles[i, 0], nearestTiles[i, 1], moldColor);
                    bacteriasStatusTwo[x, y] -= lifePointToDuplicate;
                }
            }

            bacteriasStatusTwo[x, y] -= lifePointIdle;

            if (bacteriasStatusTwo[x, y] <= 0)
                Dead(x, y);
        }

        private int intHolder;
        int[,] initialTiles = new int[4, 2];

        private int[,] GetNearestTiles(int x, int y)
        {
            initialTiles[0, 0] = x;
            intHolder = y + 1 >= CANVAS_SIZE ? -1 : y + 1;
            initialTiles[0, 1] = intHolder;

            intHolder = x + 1 >= CANVAS_SIZE ? -1 : x + 1;
            initialTiles[1, 0] = intHolder;
            initialTiles[1, 1] = y;

            initialTiles[2, 0] = x;
            intHolder = y - 1 < 0 ? -1 : y - 1;
            initialTiles[2, 1] = intHolder;

            intHolder = x - 1 < 0 ? -1 : x - 1;
            initialTiles[3, 0] = intHolder;
            initialTiles[3, 1] = y;

            return initialTiles;
        }
        
        int initialIndex;
        int finalIndex;

        int tempX;
        int tempY;
        
        private void ShuffleTiles(int[,] tileToShuffle)
        {
            int arrayLength = tileToShuffle.GetLength(0);
            if(arrayLength == 1) return;
            
            for (int i = 0; i < arrayLength; i++)
            {
                initialIndex = rnd.Next(0, arrayLength);
                
                do
                {
                    finalIndex = rnd.Next(0, arrayLength);
                } 
                while (finalIndex == initialIndex);

                tempX = tileToShuffle[initialIndex, 0];
                tempY = tileToShuffle[initialIndex, 1];
                
                tileToShuffle[initialIndex, 0] = tileToShuffle[finalIndex, 0];
                tileToShuffle[initialIndex, 1] = tileToShuffle[finalIndex, 1];
                
                tileToShuffle[finalIndex, 0] = tempX;
                tileToShuffle[finalIndex, 1] = tempY;
            }
        }

        private void Dead(int x, int y)
        {
            bacteriasStatusTwo[x, y] = - rnd.Next(minTimeBeforeFertilization, maxTimeBeforeFertilization);
            bitmap.SetPixel(x, y, deadColorTo);
        }
        

        #region Comands

        private void Canvas_Click(object sender, EventArgs e)
        {
            var positionX = MousePosition.X - Left - 8;
            var positionY = MousePosition.Y - Top - 31;

            if (positionX >= 0 && positionX < CANVAS_SIZE && positionY >= 0 && positionY < CANVAS_SIZE)
                bacteriasStatusOne[positionX, positionY] = rnd.Next(minLife, maxLife);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (gameIsStopped) return;

            Update.Stop();
            bacteriasStatusOne = new int[CANVAS_SIZE, CANVAS_SIZE];
            bacteriasStatusTwo = (int[,]) bacteriasStatusOne.Clone();
            SetupBackGroundColor();
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
            if(lowGraphics)
                SetupBackGroundColor();
        }

        #endregion

        #region Bacteria options

        private void LifeForDuplication_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (int.TryParse(LifeForDuplication.Text, out var val))
                    lifePointToDuplicate = val;
        }

        private void LifeForIdle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (int.TryParse(LifeForIdle.Text, out var val))
                    lifePointIdle = val;
        }

        private void MaxLife_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)        
                if (int.TryParse(MaxLife.Text, out var val))
                    maxLife = val;
        }

        private void MinLife_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)     
                if (int.TryParse(MinLife.Text, out var val))
                    minLife = val;
        }

        private void LifeCombo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)     
                if (int.TryParse(LifeCombo.Text, out var val))
                    lifeNearestCombo = val;
        }

        private void DupChance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(DupChance.Text, out var val))
                    duplicationChance = val;
        }
        
        private void DupTimes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(DupTimes.Text, out var val))
                    timeOfDuplications = val;
        }

        #endregion

        #region Fertilization options

        private void MaxFertiliz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(MaxFertiliz.Text, out var val))
                    maxTimeBeforeFertilization = val;
        }

        private void MinFertiliz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(MinFertiliz.Text, out var val))
                    minTimeBeforeFertilization = val;
        }

        private void FerChanc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(FerChanc.Text, out var val))
                    fertilizationChance = val;
        }

        #endregion
    }
}