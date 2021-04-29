using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace WFA_Test1
{
    public partial class Form1 : Form
    {
        private bool _gameIsStopped = false;

        private const short CanvasSize = 600;
        private const short CanvasSizeForLoop = 599;

        private byte _lifePointToDuplicate = 4;
        private byte _lifePointIdle = 2;

        private byte _minLife = 4;
        private byte _maxLife = 8;
        private float _lifeNearestCombo = 0.5f;

        private byte _duplicationChance = 40;
        private byte _timeOfDuplications = 4;

        private byte _maxTimeBeforeFertilization = 30;
        private byte _minTimeBeforeFertilization =  5;
        private byte _fertilizationChance = 50;
        
        private bool _lowGraphics = false;

        private readonly Color _moldColor = Color.FromArgb(229, 245, 88);
        private readonly Color _bgColor = Color.FromArgb(30, 30, 30);
        private readonly Color _deadColorFrom = Color.FromArgb(100, 100, 100);
        private readonly Color _deadColorTo = Color.FromArgb(30, 30, 30);

        private readonly Bitmap _bitmap = new Bitmap(CanvasSize, CanvasSize);
        private readonly Random _rnd = new Random();

        private short[,] _bacteriasStatusOne = new short[CanvasSize, CanvasSize];
        private short[,] _bacteriasStatusTwo = new short[CanvasSize, CanvasSize];

        public Form1()
        {
            InitializeComponent();
            SetupBackGroundColor();
            SetupOptionsValue();
            Update.Start();
        }

        private void SetupBackGroundColor()
        {
            for (short x = 0; x < CanvasSizeForLoop; x++)
            {
                for (short y = 0; y < CanvasSizeForLoop; y++)
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

        private short _bacteriasNumber = 0; 
        private void EvolveBacteria()
        {
            _bacteriasNumber = 0;
            
            for (short x = 0; x < CanvasSizeForLoop; x++)
            {
                for (short y = 0; y < CanvasSizeForLoop; y++)
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
            
            _bacteriasStatusOne = (short[,]) _bacteriasStatusTwo.Clone();
            _bacteriasStatusTwo = (short[,]) _bacteriasStatusOne.Clone();
        }

        private void Fertilize(short x, short y)
        {
            _intHolder = _rnd.Next(0, 100) < _fertilizationChance ? 1 : 0;
            _bacteriasStatusTwo[x, y] += (short) _intHolder;

            if(!_lowGraphics)
                _bitmap.SetPixel(x, y, GetColorInterpolation(_bacteriasStatusTwo[x, y]));
        }

        private Color GetColorInterpolation(short levelOfFertilization)
        {
            return Color.FromArgb
            (
                GetProportion(-_maxTimeBeforeFertilization, levelOfFertilization, _deadColorFrom.R - _deadColorTo.R) + _deadColorTo.R,
                GetProportion(-_maxTimeBeforeFertilization, levelOfFertilization, _deadColorFrom.G - _deadColorTo.G) + _deadColorTo.G,
                GetProportion(-_maxTimeBeforeFertilization, levelOfFertilization, _deadColorFrom.B - _deadColorTo.B) + _deadColorTo.B
            );
        }

        private short GetProportion(float num1, float num2, float num3)
        {
            var result = num3 * (num2 / num1);
            return (short) result;
        }

        private void Duplicate(short x, short y)
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
                    
                    float lifeBonus = 1;
                    for (byte b = 0; b < nearestTiles.GetLength(0); b++)
                    {
                        if (nearestTiles[b, 0] != -1 && nearestTiles[b, 1] != -1)
                        {
                            if (_bacteriasStatusOne[nearestTiles[b, 0], nearestTiles[b, 1]] > 0)
                                lifeBonus += _lifeNearestCombo;
                        }
                    }

                    _bacteriasStatusTwo[nearestTiles[i, 0], nearestTiles[i, 1]] =
                        (short) (_rnd.Next(_minLife, _maxLife) * lifeBonus);

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
        short[,] _initialTiles = new short[4, 2];

        private short[,] GetNearestTiles(short x, short y)
        {
            _initialTiles[0, 0] = x;
            _intHolder = y + 1 >= CanvasSize ? -1 : y + 1;
            _initialTiles[0, 1] = (short) _intHolder;

            _intHolder = x + 1 >= CanvasSize ? -1 : x + 1;
            _initialTiles[1, 0] = (short) _intHolder;
            _initialTiles[1, 1] = y;

            _initialTiles[2, 0] = x;
            _intHolder = y - 1 < 0 ? -1 : y - 1;
            _initialTiles[2, 1] = (short) _intHolder;

            _intHolder = x - 1 < 0 ? -1 : x - 1;
            _initialTiles[3, 0] = (short) _intHolder;
            _initialTiles[3, 1] = y;

            return _initialTiles;
        }
        
        byte _initialIndex;
        byte _finalIndex;

        short _tempX;
        short _tempY;
        
        private void ShuffleTiles(short[,] tileToShuffle)
        {
            byte arrayLength = (byte) tileToShuffle.GetLength(0);
            if(arrayLength == 1) return;
            
            for (byte i = 0; i < arrayLength; i++)
            {
                _initialIndex = (byte) _rnd.Next(0, arrayLength);
                
                do
                {
                    _finalIndex = (byte) _rnd.Next(0, arrayLength);
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

        private void Dead(short x, short y)
        {
            _bacteriasStatusTwo[x, y] = (short) - _rnd.Next(_minTimeBeforeFertilization, _maxTimeBeforeFertilization);
            _bitmap.SetPixel(x, y, _deadColorTo);
        }
        

        #region Comands

        private void Canvas_Click(object sender, EventArgs e)
        {
            var positionX = MousePosition.X - Left - 8;
            var positionY = MousePosition.Y - Top - 31;

            if (positionX >= 0 && positionX < CanvasSize && positionY >= 0 && positionY < CanvasSize)
                _bacteriasStatusOne[positionX, positionY] = (short) _rnd.Next(_minLife, _maxLife);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (_gameIsStopped) return;

            Update.Stop();
            _bacteriasStatusOne = new short[CanvasSize, CanvasSize];
            _bacteriasStatusTwo = (short[,]) _bacteriasStatusOne.Clone();
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
                    _lifePointToDuplicate = (byte) val;
        }

        private void LifeForIdle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (int.TryParse(LifeForIdle.Text, out var val))
                    _lifePointIdle = (byte) val;
        }

        private void MaxLife_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)        
                if (int.TryParse(MaxLife.Text, out var val))
                    _maxLife = (byte) val;
        }

        private void MinLife_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)     
                if (int.TryParse(MinLife.Text, out var val))
                    _minLife = (byte) val;
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
                    _duplicationChance = (byte) val;
        }
        
        private void DupTimes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(DupTimes.Text, out var val))
                    _timeOfDuplications = (byte) val;
        }

        #endregion

        #region Fertilization options

        private void MaxFertiliz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(MaxFertiliz.Text, out var val))
                    _maxTimeBeforeFertilization = (byte) val;
        }

        private void MinFertiliz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(MinFertiliz.Text, out var val))
                    _minTimeBeforeFertilization = (byte) val;
        }

        private void FerChanc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)   
                if (int.TryParse(FerChanc.Text, out var val))
                    _fertilizationChance = (byte) val;
        }

        #endregion
    }
}