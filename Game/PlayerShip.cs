using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NeonShooter
{

    class PlayerShip : Entity
    {
        // instance attributes define PlayerShip as a singleton
        private static PlayerShip instance;
        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerShip();

                return instance;
            }
        }
        int framesUntilRespawn = 0;
        public bool IsDead { get { return framesUntilRespawn > 0; } }
        const int cooldownFrames = 6;
        int cooldownRemaining = 0;
        static Random rand = new Random();

        private PlayerShip()
        {
            image = Art.Player;
            Position = NeonShooterGame.ScreenSize / 2; // center player
            Radius = 10; // used for collision
        }

        public override void Update()
        {
            if (IsDead)
            {
                framesUntilRespawn--;
                return;
            }

            const float speed = 8;
            Velocity = speed * Input.GetMovementDirection();

            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, NeonShooterGame.ScreenSize - Size / 2);

            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            var aim = Input.GetAimDirection();

            if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
            {
                cooldownRemaining = cooldownFrames;
                float aimAngle = aim.ToAngle();
                // Quaternions are used for rotation
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                // see MathUtils.cs and Extensions (NextFload()) for reference
                // on these helper methods
                float randomSpread = rand.NextFloat(-0.04f, 0.04f) + rand.NextFloat(-0.04f, 0.04f);
                Vector2 vel = MathUtil.FromPolar(aimAngle + randomSpread, 11f);

                // Shoot two parallel bullets
                Vector2 offset = Vector2.Transform(new Vector2(35, -8), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel));

                offset = Vector2.Transform(new Vector2(35, 8), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel));

            }

            if (cooldownRemaining > 0)
                cooldownRemaining--;
        }

        public void Kill()
        {
            framesUntilRespawn = 60;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
                base.Draw(spriteBatch);
        }

    }

}