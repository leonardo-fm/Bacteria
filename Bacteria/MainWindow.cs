using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Bacteria
{
    public partial class MainWindow : Form
    {
        private bool _gameIsStopped = false;

        private const int CanvasSize = 600;
        private const int CanvasSizeForLoop = 599;

        private int _lifePointToDuplicate = 4;
        private int _lifePointIdle = 2;

        private int _minLife = 3;
        private int _maxLife = 8;
        private double _lifeNearestCombo = 0.5f;

        private int _duplicationChance = 40;
        private int _timeOfDuplications = 1;

        private int _maxTimeBeforeFertilization = 30;
        private int _minTimeBeforeFertilization =  5;
        private int _fertilizationChance = 40;
        
        private bool _lowGraphics = false;

        private readonly Color _moldColor = Color.FromArgb(229, 245, 88);
        private readonly Color _bgColor = Color.FromArgb(30, 30, 30);
        private readonly Color _deadColorFrom = Color.FromArgb(100, 100, 100);
        private readonly Color _deadColorTo = Color.FromArgb(30, 30, 30);

        private readonly Bitmap _bitmap = new Bitmap(CanvasSize, CanvasSize);
        private readonly Random _rnd = new Random();

        private int[,] _bacteriasStatusOne = new int[CanvasSize, CanvasSize];
        private int[,] _bacteriasStatusTwo = new int[CanvasSize, CanvasSize];

        public MainWindow()
        {
            InitializeComponent();
            SetupBackGroundColor();
            SetupOptionsValue();
            Update.Start();
        }

        private void SetupBackGroundColor()
        {
            for (int x = 0; x < CanvasSizeForLoop; x++)
            {
                for (int y = 0; y < CanvasSizeForLoop; y++)
                {
                    _bitmap.SetPixel(x, y, _bgColor);
                }
            }

            Canvas.Image = _bitmap;
        }
        private void SetupOptionsValue()
        {
            LifeForDuplication.Text = _lifePointToDuplicate.ToString();
            LifeForIdle.Text = _lifePointIdle.ToString();
            MaxLife.Text = _maxLife.ToString();
            MinLife.Text = _minLife.ToString();
            LifeCombo.Text = _lifeNearestCombo.ToString(CultureInfo.InvariantCulture);
            DupChance.Text = _duplicationChance.ToString();
            DupTimes.Text = _timeOfDuplications.ToString();
            
            MaxFertiliz.Text = _maxTimeBeforeFertilization.ToString();
            MinFertiliz.Text = _minTimeBeforeFertilization.ToString();
            FerChanc.Text = _fertilizationChance.ToString();
            
            NumOfBacterias.Text = _bacteriasNumber.ToString();
        }

        private void Update_Tick(object sender, EventArgs e)
        {
            EvolveBacteria();
        }

        private int _bacteriasNumber = 0; 
        private void EvolveBacteria()
        {
            _bacteriasNumber = 0;
            
            for (int x = 0; x < CanvasSizeForLoop; x++)
            {
                for (int y = 0; y < CanvasSizeForLoop; y++)
                {
                    if (_bacteriasStatusOne[x, y] == 0) continue;

                    if (_bacteriasStatusOne[x, y] < 0)
                    {
                        Fertilize(x, y);
                        continue;
                    }

                    Duplicate(x, y);
                }
            }

            Canvas.Image = _bitmap;
            NumOfBacterias.Text = _bacteriasNumber.ToString();
            
            _bacteriasStatusOne = (int[,]) _bacteriasStatusTwo.Clone();
            _bacteriasStatusTwo = (int[,]) _bacteriasStatusOne.Clone();
        }

        private void Fertilize(int x, int y)
        {
            _intHolder = _rnd.Next(0, 100) < _fertilizationChance ? 1 : 0;
            _bacteriasStatusTwo[x, y] += (int) _intHolder;

            if(!_lowGraphics)
                _bitmap.SetPixel(x, y, GetColorInterpolation(_bacteriasStatusTwo[x, y]));
        }

        private Color GetColorInterpolation(int levelOfFertilization)
        {
            return Color.FromArgb
            (
                GetProportion(-_maxTimeBeforeFertilization, levelOfFertilization, _deadColorFrom.R - _deadColorTo.R) + _deadColorTo.R,
                GetProportion(-_maxTimeBeforeFertilization, levelOfFertilization, _deadColorFrom.G - _deadColorTo.G) + _deadColorTo.G,
                GetProportion(-_maxTimeBeforeFertilization, levelOfFertilization, _deadColorFrom.B - _deadColorTo.B) + _deadColorTo.B
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
            var duplicationNumber = _timeOfDuplications;

            for (var i = 0; i < nearestTiles.GetLength(0); i++)
            {
                if (nearestTiles[i, 0] != -1 && nearestTiles[i, 1] != -1)
                {
                    if (_bacteriasStatusOne[nearestTiles[i, 0], nearestTiles[i, 1]] != 0) continue;
                    if (_rnd.Next(100) >= _duplicationChance) continue;

                    if(duplicationNumber == 0) continue;
                    duplicationNumber -= 1;
                    
                    double lifeBonus = 1;
                    for (int b = 0; b < nearestTiles.GetLength(0); b++)
                    {
                        if (nearestTiles[b, 0] != -1 && nearestTiles[b, 1] != -1)
                        {
                            if (_bacteriasStatusOne[nearestTiles[b, 0], nearestTiles[b, 1]] > 0)
                                lifeBonus += _lifeNearestCombo;
                        }
                    }

                    _bacteriasStatusTwo[nearestTiles[i, 0], nearestTiles[i, 1]] =
                        (int) (_rnd.Next(_minLife, _maxLife) * lifeBonus);

                    _bacteriasNumber++;

                    _bitmap.SetPixel(nearestTiles[i, 0], nearestTiles[i, 1], _moldColor);
                    _bacteriasStatusTwo[x, y] -= _lifePointToDuplicate;
                }
            }

            _bacteriasStatusTwo[x, y] -= _lifePointIdle;

            if (_bacteriasStatusTwo[x, y] <= 0)
                Dead(x, y);
        }

        private int _intHolder;
        int[,] _initialTiles = new int[4, 2];

        private int[,] GetNearestTiles(int x, int y)
        {
            _initialTiles[0, 0] = x;
            _intHolder = y + 1 >= CanvasSize ? -1 : y + 1;
            _initialTiles[0, 1] = (int) _intHolder;

            _intHolder = x + 1 >= CanvasSize ? -1 : x + 1;
            _initialTiles[1, 0] = (int) _intHolder;
            _initialTiles[1, 1] = y;

            _initialTiles[2, 0] = x;
            _intHolder = y - 1 < 0 ? -1 : y - 1;
            _initialTiles[2, 1] = (int) _intHolder;

            _intHolder = x - 1 < 0 ? -1 : x - 1;
            _initialTiles[3, 0] = (int) _intHolder;
            _initialTiles[3, 1] = y;

            return _initialTiles;
        }
        
        int _initialIndex;
        int _finalIndex;

        int _tempX;
        int _tempY;
        
        private void ShuffleTiles(int[,] tileToShuffle)
        {
            int arrayLength = (int) tileToShuffle.GetLength(0);
            if(arrayLength == 1) return;
            
            for (int i = 0; i < arrayLength; i++)
            {
                _initialIndex = (int) _rnd.Next(0, arrayLength);
                
                do
                {
                    _finalIndex = (int) _rnd.Next(0, arrayLength);
                } 
                while (_finalIndex == _initialIndex);

                _tempX = tileToShuffle[_initialIndex, 0];
                _tempY = tileToShuffle[_initialIndex, 1];
                
                tileToShuffle[_initialIndex, 0] = tileToShuffle[_finalIndex, 0];
                tileToShuffle[_initialIndex, 1] = tileToShuffle[_finalIndex, 1];
                
                tileToShuffle[_finalIndex, 0] = _tempX;
                tileToShuffle[_finalIndex, 1] = _tempY;
            }
        }

        private void Dead(int x, int y)
        {
            _bacteriasStatusTwo[x, y] = (int) - _rnd.Next(_minTimeBeforeFertilization, _maxTimeBeforeFertilization);
            _bitmap.SetPixel(x, y, _deadColorTo);
        }
        

        #region Comands

        private void Canvas_Click(object sender, EventArgs e)
        {
            var positionX = MousePosition.X - Left - 8;
            var positionY = MousePosition.Y - Top - 31;

            if (positionX >= 0 && positionX < CanvasSize && positionY >= 0 && positionY < CanvasSize)
                _bacteriasStatusOne[positionX, positionY] = (int) _rnd.Next(_minLife, _maxLife);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (_gameIsStopped) return;

            Update.Stop();
            _bacteriasStatusOne = new int[CanvasSize, CanvasSize];
            _bacteriasStatusTwo = (int[,]) _bacteriasStatusOne.Clone();
            SetupBackGroundColor();
            Update.Start();

            Console.WriteLine("--- Reset ---");
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (!_gameIsStopped)
            {
                Update.Stop();
                _gameIsStopped = true;
                btnPause.Text = "Resume";
                Console.WriteLine("--- Pause ---");
            }
            else
            {
                Update.Start();
                _gameIsStopped = false;
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
            _lowGraphics = LowGraph.Checked;
            if(_lowGraphics)
                SetupBackGroundColor();
        }

        #endregion

        #region Bacteria options

        private void LifeForDuplication_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (int.TryParse(LifeForDuplication.Text, out var val))
                    _lifePointToDuplicate = (int) val;
        }

        private void LifeForIdle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (int.TryParse(LifeForIdle.Text, out var val))
                    _lifePointIdle = (int) val;
        }

        private void MaxLife_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)        
                if (int.TryParse(MaxLife.Text, out var val))
                    _maxLife = (int) val;
        }

        private void MinLife_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)     
                if (int.TryParse(MinLife.Text, out var val))
                    _minLife = (int) val;
        }

        private void LifeCombo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)     
                if (int.TryParse(LifeCombo.Text, out var val))
                    _lifeNearestCombo = val;
        }

        private void DupChance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(DupChance.Text, out var val))
                    _duplicationChance = (int) val;
        }
        
        private void DupTimes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(DupTimes.Text, out var val))
                    _timeOfDuplications = (int) val;
        }

        #endregion

        #region Fertilization options

        private void MaxFertiliz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(MaxFertiliz.Text, out var val))
                    _maxTimeBeforeFertilization = (int) val;
        }

        private void MinFertiliz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(MinFertiliz.Text, out var val))
                    _minTimeBeforeFertilization = (int) val;
        }

        private void FerChanc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(FerChanc.Text, out var val))
                    _fertilizationChance = (int) val;
        }

        #endregion
    }
}