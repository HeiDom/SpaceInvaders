using SpaceInvaders.Games;
using SpaceInvaders.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Actors.Player
{
    public class Ship
    {
        #region - Vars -

        private const int BlocWidth = 15;
        private const int BlocHeight = 15;

        private Game _game;
        private Rectangle[] _ship;

        private readonly GraphicUtil _graphicUtil;

        #endregion

        #region - Get/Set - 

        private int X { get; set; }
        private int Y { get; set; }
        
        public int Width { get; private set; }
        public int Height { get; private set; }

        #endregion

        #region - Constructor -

        /// <summary>
        /// Stellt eine Instanz des Objekt Ship bereit
        /// </summary>
        /// <param name="game"></param>
        public Ship(Game game)
        {
            _game = game;
            _graphicUtil = new GraphicUtil(game);
        }

        #endregion

        #region - Create - 

        /// <summary>
        /// Erstellt ein Raumschiff
        /// </summary>
        public void Create()
        {
            _ship = new Rectangle[3];

            int y = _game.ContainerHeight / 2; //- BlocHeight;
            int x = (_game.ContainerWidth / 2) - (BlocWidth * 3);

            _ship[0] = _graphicUtil.CreateRectangle(x, y, BlocWidth * 7, BlocHeight * 2);
            _ship[1] = _graphicUtil.CreateRectangle(_ship[0].X + (BlocWidth * 2), _ship[0].Y - BlocHeight, BlocWidth * 3, BlocHeight);
            _ship[2] = _graphicUtil.CreateRectangle(_ship[1].X + BlocWidth, _ship[1].Y - BlocHeight, BlocWidth, BlocHeight);
            _graphicUtil.FillRectangles(_ship, Color.Green);
        }

        #endregion
    }
}
