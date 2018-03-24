using System;

namespace SpaceInvaders.Util
{
    public class RandomUtil
    {
        #region - Vars -

        private static readonly Random Random = new Random();

        #endregion
        
        #region - Generate -

        /// <summary>
        /// Generiert eine zufällige Zahl, ist dafür ausgelegt auf kurzer Zeit viele verschiedene zahlen zu generieren
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int Generate(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

        #endregion
    }
}
