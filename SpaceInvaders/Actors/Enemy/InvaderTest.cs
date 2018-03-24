using System;
using System.Drawing;
using SpaceInvaders.Games;
using SpaceInvaders.Util;

namespace SpaceInvaders.Actors.Enemy
{
    public class InvaderTest
    {
        private Game _game;
        private Rectangle[] _invader;
        private readonly GraphicUtil _graphicUtil;

        private int X { get; set; }
        private int Y { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }
        
        /// <summary>
        /// Stellt eine Instanz der Klasse InvaderTest bereit
        /// </summary>
        /// <param name="game"></param>
        public InvaderTest(Game game)
        {
            _game = game;
            _graphicUtil = new GraphicUtil(game);

            Width = 3;
            Height = 3;
            X = _game.ContainerWidth / 2;
            Y = _game.ContainerHeight / 2;

            CreateMid();
        }

        private void CreateMid()
        {
            _invader = new Rectangle[12];

            var blocWidth = Width * 2;
            var blocHeight = Height * 5;

            _invader[0] = _graphicUtil.CreateRectangle(X, Y, blocWidth, blocHeight);
            _graphicUtil.FillRectangle(_invader[0], Color.Red);

            CreateLeftEye();
        }

        private void CreateLeftEye()
        {
            X -= Width;
            Y += Height;

            var blocHeight = Height * 2;

            _invader[1] = _graphicUtil.CreateRectangle(X, Y, Width, blocHeight);
            _graphicUtil.FillRectangle(_invader[1], Color.Red);


            Y = Y + (Height * 3);
            _invader[2] = _graphicUtil.CreateRectangle(X, Y, Width, blocHeight);
            _graphicUtil.FillRectangle(_invader[2], Color.Red);

            CreateRightEye();
        }

        private void CreateRightEye()
        {
            Y = _invader[1].Y;
            X = X + (Width * 3);

            var blocHeight = Height * 2;

            _invader[3] = _graphicUtil.CreateRectangle(X, Y, Width, blocHeight);
            _graphicUtil.FillRectangle(_invader[3], Color.Red);

            Y = Y + (Height * 3);
            _invader[4] = _graphicUtil.CreateRectangle(X, Y, Width, blocHeight);
            _graphicUtil.FillRectangle(_invader[4], Color.Red);

            CreateLeftSide();
        }

        private void CreateLeftSide()
        {
            Y = _invader[1].Y + Height;
            X = X - Width * 4;

            var blocHeight = Height * 4;

            _invader[5] = _graphicUtil.CreateRectangle(X, Y, Width, blocHeight);
            _graphicUtil.FillRectangle(_invader[5], Color.Red);

            X -= Width;
            Y = Y + Height;
            blocHeight = Height * 2;

            _invader[6] = _graphicUtil.CreateRectangle(X, Y, Width, blocHeight);
            _graphicUtil.FillRectangle(_invader[6], Color.Red);

            CreateRightSide();
        }

        private void CreateRightSide()
        {
            Y = _invader[1].Y + Height;
            X = _invader[3].X + Width;

            var blocHeight = Height * 4;

            _invader[7] = _graphicUtil.CreateRectangle(X, Y, Width, blocHeight);
            _graphicUtil.FillRectangle(_invader[7], Color.Red);

            X += Width;
            Y = Y + Height;
            blocHeight = Height * 2;

            _invader[8] = _graphicUtil.CreateRectangle(X, Y, Width, blocHeight);
            _graphicUtil.FillRectangle(_invader[8], Color.Red);

            CreateLeftArm();
        }

        private void CreateLeftArm()
        {
            X = _invader[6].X;
            Y = _invader[6].Y + Height * 3;
            
            _invader[9] = _graphicUtil.CreateRectangle(X, Y, Width, Height);
            _graphicUtil.FillRectangle(_invader[9], Color.Red);

            X = _invader[9].X + Width;
            Y = _invader[9].Y + Height;

            _invader[10] = _graphicUtil.CreateRectangle(X, Y, Width, Height);
            _graphicUtil.FillRectangle(_invader[10], Color.Red);

            CreateRightArm();
        }

        private void CreateRightArm()
        {
            X = _invader[8].X;
            Y = _invader[8].Y + Height * 3;

            _invader[11] = _graphicUtil.CreateRectangle(X, Y, Width, Height);
            _graphicUtil.FillRectangle(_invader[11], Color.Red);

            X = _invader[11].X - Width;
            Y = _invader[11].Y + Height;

            _invader[12] = _graphicUtil.CreateRectangle(X, Y, Width, Height);
            _graphicUtil.FillRectangle(_invader[12], Color.Red);
        }
    }
}
