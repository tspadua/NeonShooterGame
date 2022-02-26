using Microsoft.Xna.Framework;
using System;

namespace NeonShooter
{

    static class EnemySpawner
    {
        static Random rand = new Random();

        // SpawnChance is inverse because the chance gets higher
        // as it gets closer to 1
        static float inverseSpawnChance = 60;

        public static void Update()
        {
            // Each frame, there is a one in inverseSpawnChance of generating each type of enemy.
            // The chance of spawning an enemy gradually increases until it reaches a maximum of one in twenty.
            if (!PlayerShip.Instance.IsDead && EntityManager.Count < 200)
            {
                if (rand.Next((int)inverseSpawnChance) == 0)
                    EntityManager.Add(Enemy.CreateSeeker(GetSpawnPosition()));

                if (rand.Next((int)inverseSpawnChance) == 0)
                    EntityManager.Add(Enemy.CreateWanderer(GetSpawnPosition()));
            }

            // Slowly increase the InverseSpawnChance to a max of twenty
            if (inverseSpawnChance > 20)
                inverseSpawnChance -= 0.005f;
        }

        private static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(rand.Next((int)NeonShooterGame.ScreenSize.X), rand.Next((int)NeonShooterGame.ScreenSize.Y));
            }

            // The while loop will work efficiently as long as the area in which enemies can spawn
            // is bigger than the area where they can't spawn.
            // However, if you make the forbidden area too large, you will get an infinite loop.

            // Make the enemy spawn at least 250 pixels away.
            // Since we used DistanceSquared we should also compare it to 250 squared
            while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 250 * 250);

            return pos;
        }

        public static void Reset()
        {
            inverseSpawnChance = 60;
        }

    }


}