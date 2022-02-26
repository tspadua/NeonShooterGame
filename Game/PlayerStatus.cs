using Microsoft.Xna.Framework;

namespace NeonShooter
{
    static class PlayerStatus
    {
        // amount of time for a multiplier to expire (in seconds)
        private const float multiplierExpiryTime = 0.8f;
        private const int maxMultiplier = 20;

        public static int Lives { get; private set; }
        public static int Score { get; private set; }
        public static int Multiplier { get; private set; }

        private static float MultiplierTimeLeft;
        private static int scoreForExtraLife;

        static PlayerStatus()
        {
            Reset();
        }

        public static void Reset()
        {
            Score = 0;
            Multiplier = 1;
            Lives = 4;
            scoreForExtraLife = 2000;
            MultiplierTimeLeft = 0;
        }

        public static void Update()
        {
            if (Multiplier > 1)
            {
                // update the multiplier timer
                if ((MultiplierTimeLeft -= (float)NeonShooterGame.GameTime.ElapsedGameTime.TotalSeconds) <= 0)
                {
                    MultiplierTimeLeft = multiplierExpiryTime;
                    ResetMultiplier();
                }
            }
        }

        public static void AddPoints(int basePoints)
        {
            if (PlayerShip.Instance.IsDead)
                return;

            Score += basePoints * Multiplier;
            while (Score >= scoreForExtraLife)
            {
                scoreForExtraLife += 2000;
                Lives++;
            }
        }

        public static void IncreaseMultiplier()
        {
            if (PlayerShip.Instance.IsDead)
                return;

            MultiplierTimeLeft = multiplierExpiryTime;

            if (Multiplier < maxMultiplier)
                Multiplier++;
        }

        public static void ResetMultiplier()
        {
            Multiplier = 1;
        }

        public static void RemoveLife()
        {
            Lives--;
        }
    }
}