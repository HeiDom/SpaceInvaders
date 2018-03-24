using SpaceInvaders.Games;
using SpaceInvaders.Util;
using System.Drawing;

namespace SpaceInvaders.Actors.Enemy
{
    public class Invader
    {
        #region - Vars -

        private Rectangle[] _invader;
        //private const int BlocWidth = 25;
        //private const int BlocHeight = 25;
        private const int BlocWidth = 4;
        private const int BlocHeight = 4;
        private const int BlocLengthY = 5;
        private const int BlocLengthX = 8;

        public int Width { get; private set; }
        public int Height { get; private set; }


        private int _startX;
        private int _startY;

        private int X { get; set; }
        private int Y { get; set; }

        private readonly Color _color;
        private readonly Color _removeColor;
        private readonly GraphicUtil _graphicUtil;

        #endregion

        #region - Constuctor -

        /// <summary>
        /// Konstruktor des Invader
        /// </summary>
        /// <param name="game"></param>
        public Invader(Game game)
        {
            Alive = true;
            _color = Color.Red;
            _removeColor = Color.Black;
            _graphicUtil = new GraphicUtil(game);
        }

        #endregion

        #region - Create -

        /// <summary>
        /// Erstellt ein Invader an den übergeben X- und Y-Koordinaten
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Create(int x, int y)
        {
            X = x;
            Y = y;

            Width = BlocWidth * BlocLengthY;
            Height = BlocHeight * BlocLengthX;

            _startX = X;
            _startY = Y;

            CreateMid();
            CreateLeftEye();
            CreateRightEye();
            CreateLeftSide();
            CreateRightSide();
            CreateLeftArm();
            CreateRightArm();
        }

        /// <summary>
        /// Erstellt die Mitte des Invaders
        /// </summary>
        private void CreateMid()
        {
            _invader = new Rectangle[13];

            const int blocWidth = BlocWidth * 2;
            const int blocHeight = BlocHeight * 5;

            _invader[0] = _graphicUtil.CreateRectangle(X, Y, blocWidth, blocHeight);
            _graphicUtil.FillRectangle(_invader[0], _color);
        }

        /// <summary>
        /// Erstellt das Linke Auge des Invaders
        /// </summary>
        private void CreateLeftEye()
        {
            X -= BlocWidth;
            Y += BlocHeight;

            const int blocHeight = BlocHeight * 2;

            _invader[1] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, blocHeight);
            _graphicUtil.FillRectangle(_invader[1], _color);


            Y = Y + (BlocHeight * 3);
            _invader[2] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, blocHeight);
            _graphicUtil.FillRectangle(_invader[2], _color);

        }

        /// <summary>
        /// Ersellt das Rechte Auge des Invaders
        /// </summary>
        private void CreateRightEye()
        {
            Y = _invader[1].Y;
            X = X + (BlocWidth * 3);

            const int blocHeight = BlocHeight * 2;

            _invader[3] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, blocHeight);
            _graphicUtil.FillRectangle(_invader[3], _color);

            Y = Y + (BlocHeight * 3);
            _invader[4] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, blocHeight);
            _graphicUtil.FillRectangle(_invader[4], _color);
        }

        /// <summary>
        /// Erstellt die Linke Seite des Invader
        /// </summary>
        private void CreateLeftSide()
        {
            Y = _invader[1].Y + BlocHeight;
            X = X - BlocWidth * 4;

            var blocHeight = BlocHeight * 4;

            _invader[5] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, blocHeight);
            _graphicUtil.FillRectangle(_invader[5], _color);

            X -= BlocWidth;
            Y = Y + BlocHeight;
            blocHeight = BlocHeight * 2;

            _invader[6] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, blocHeight);
            _graphicUtil.FillRectangle(_invader[6], _color);

        }

        /// <summary>
        /// Erstellt die Rechte Seite des Invaders
        /// </summary>
        private void CreateRightSide()
        {
            Y = _invader[1].Y + BlocHeight;
            X = _invader[3].X + BlocWidth;

            var blocHeight = BlocHeight * 4;

            _invader[7] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, blocHeight);
            _graphicUtil.FillRectangle(_invader[7], _color);

            X += BlocWidth;
            Y = Y + BlocHeight;
            blocHeight = BlocHeight * 2;

            _invader[8] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, blocHeight);
            _graphicUtil.FillRectangle(_invader[8], _color);

        }

        /// <summary>
        /// Ersellt den Linken Arm des Invaders
        /// </summary>
        private void CreateLeftArm()
        {
            X = _invader[6].X;
            Y = _invader[6].Y + BlocHeight * 3;

            _invader[9] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, BlocHeight);
            _graphicUtil.FillRectangle(_invader[9], _color);

            X = _invader[9].X + BlocWidth;
            Y = _invader[9].Y + BlocHeight;

            _invader[10] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, BlocHeight);
            _graphicUtil.FillRectangle(_invader[10], _color);

        }

        /// <summary>
        /// Erstellt den Rechten Arm des Invaders
        /// </summary>
        private void CreateRightArm()
        {
            X = _invader[8].X;
            Y = _invader[8].Y + BlocHeight * 3;

            _invader[11] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, BlocHeight);
            _graphicUtil.FillRectangle(_invader[11], _color);

            X = _invader[11].X - BlocWidth;
            Y = _invader[11].Y + BlocHeight;

            _invader[12] = _graphicUtil.CreateRectangle(X, Y, BlocWidth, BlocHeight);
            _graphicUtil.FillRectangle(_invader[12], _color);

            X = _startX;
            Y = _startY;
        }

        #endregion

        #region - Set -

        /// <summary>
        /// Setzt den zustand des Invaders auf tot
        /// </summary>
        public void Dead()
        {
            Alive = false;
        }

        /// <summary>
        /// Entfernt den Invader
        /// </summary>
        public void Remove()
        {
            _graphicUtil.FillRectangles(_invader, _removeColor);
        }

        /// <summary>
        /// Sezt eine neue Location des Invaders
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Move(int x, int y)
        {
            if (!Alive)
                return;

            Remove();

            X += x;
            Y += y;

            for (var i = 0; i < _invader.Length; i++)
            {
                _invader[i].X += x;
                _invader[i].Y += y;

                _graphicUtil.FillRectangle(_invader[i], _color);
            }
        }

        /// <summary>
        /// Aktualisiert den Invader
        /// </summary>
        public void Refresh()
        {
            _graphicUtil.FillRectangles(_invader, _color);
        }

        #endregion

        #region - Get -

        /// <summary>
        /// Gibt an ob der Invader am Leben ist
        /// </summary>
        public bool Alive { get; private set; }
        
        /// <summary>
        /// Gibt die X-Koordinate des Invaders an
        /// </summary>
        public int LocationX
        {
            //get { return _invader.Location.X; }
            //get { return _startX; }
            get { return _invader[6].X; }
        }

        /// <summary>
        /// Gibt die Y-Koordinate des Invaders an
        /// </summary>
        public int LocationY
        {
            get { return _invader[0].Y; }
        }

        #endregion
    }
}