using System.IO;

namespace NeonShooter
{
    static class PlayerStatus
    {
        // amount of time for a multiplier to expire (in seconds)
        private const float multiplierExpiryTime = 0.8f;
        private const int maxMultiplier = 20;

        public static int Lives { get; private set; }
        public static int Score { get; private set; }
        public static int HighScore { get; private set; }
        public static int Multiplier { get; private set; }
        public static bool IsGameOver { get { return Lives == 0; } }

        private static float MultiplierTimeLeft;
        private static int scoreForExtraLife;
        private const string highScoreFilename = "highscore.txt";
        private static int LoadHighScore()
        {
            // return the saved high score if possible and return 0 otherwise
            int score;
            return File.Exists(highScoreFilename) && int.TryParse(File.ReadAllText(highScoreFilename), out score) ? score : 0;
        }

        private static void SaveHighScore(int score)
        {
            File.WriteAllText(highScoreFilename, score.ToString());
        }
        static PlayerStatus()
        {
            HighScore = LoadHighScore();
            Reset();
        }

        public static void Reset()
        {
            if (Score > HighScore)
                SaveHighScore(HighScore = Score);

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