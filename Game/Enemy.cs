using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace NeonShooter
{
    class Enemy : Entity
    {
        public static Random rand = new Random();
        private int timeUntilStart = 60;
        public int PointValue { get; private set; }
        public bool IsActive { get { return timeUntilStart <= 0; } }
        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();

        public Enemy(Texture2D image, Vector2 position)
        {
            this.image = image;
            Position = position;
            Radius = image.Width / 2f;
            color = Color.Transparent;
        }

        public override void Update()
        {
            if (timeUntilStart <= 0)
            {
                ApplyBehaviours();
            }
            else
            {
                // enemy fade in for 60 frames
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }

            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, NeonShooterGame.ScreenSize - Size / 2);

            Velocity *= 0.8f; // this fakes a friction effect
        }

        public void WasShot()
        {
            IsExpired = true;
        }

        public void AddBehaviour(IEnumerable<int> behaviour)
        {
            behaviours.Add(behaviour.GetEnumerator());
        }

        private void ApplyBehaviours()
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (!behaviours[i].MoveNext())
                    behaviours.RemoveAt(i--);
            }
        }

        public static Enemy CreateSeeker(Vector2 position)
        {
            var enemy = new Enemy(Art.Seeker, position);
            enemy.AddBehaviour(enemy.FollowPlayer());

            return enemy;
        }

        public static Enemy CreateWanderer(Vector2 position)
        {
            var enemy = new Enemy(Art.Wanderer, position);
            enemy.AddBehaviour(enemy.MoveRandomly());
            return enemy;
        }

        /// <summary><c>HandleCollision</c> push enemies away from each other</summary>
        public void HandleCollision(Enemy other)
        {
            var d = Position - other.Position;
            // LengthSquared ensures that enemies are pushed harder as get closer
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }

        #region Behaviours
        IEnumerable<int> FollowPlayer(float acceleration = 1f)
        {
            while (true)
            {
                // PlayerShip.Instance.Position - Position makes the enemy follow the player,
                // ScaleTo scales the Vector at constant acceleration rate -- see Extensions.cs for reference on ScaleTo
                Velocity += (PlayerShip.Instance.Position - Position).ScaleTo(acceleration);
                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();

                yield return 0;
            }
        }

        IEnumerable<int> MoveRandomly()
        {
            // direction is an angle, thus is maxed at 2pi
            float direction = rand.NextFloat(0, MathHelper.TwoPi);


            while (true)
            {
                direction += rand.NextFloat(-0.1f, 0.1f); // prevent sharp turns
                direction = MathHelper.WrapAngle(direction);

                for (int i = 0; i < 6; i++)
                {
                    Velocity += MathUtil.FromPolar(direction, 0.4f);
                    Orientation -= 0.05f;

                    var bounds = NeonShooterGame.Viewport.Bounds;
                    bounds.Inflate(-image.Width, -image.Height); // account for sprite size

                    // if the enemy is outside the bounds, make it move away from the edge
                    if (!bounds.Contains(Position.ToPoint()))
                        direction = (NeonShooterGame.ScreenSize / 2 - Position).ToAngle() + rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);

                    yield return 0;
                }
            }

            #endregion

        }
    }
}