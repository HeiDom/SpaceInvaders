using System;
using System.Globalization;

namespace SpaceInvaders.Util
{
    public class FrameUtil
    {
        #region - Vars -

        private static int _lastTick;
        private static int _frameRate;
        private static double _lastFrameRate;

        private const double MaxFrameRate = 60;

        #endregion

        #region - Get - 

        /// <summary>
        /// Berechnet die Frames der Form
        /// </summary>
        /// <returns></returns>
        public static string GetFrames()
        {
            if (Environment.TickCount - _lastTick >= 1000)
            {
                _lastFrameRate = _frameRate;
                _frameRate = 0;
                _lastTick = Environment.TickCount;
            }

            _frameRate++;
            
            return _lastFrameRate.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Berechnet die Bewegungsgeschwindigkeit anhand der Frames, hat noch nachholbedard die Methode
        /// </summary>
        /// <returns></returns>
        public static double CalculateMoveSpeed()
        {
            GetFrames();
            
            return _lastFrameRate / MaxFrameRate * 100;
        }

        #endregion
    }
}
