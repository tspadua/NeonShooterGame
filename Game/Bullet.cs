using Microsoft.Xna.Framework;

namespace NeonShooter
{
    class Bullet : Entity
    {
        public Bullet(Vector2 position, Vector2 velocity)
        {
            image = Art.Bullet;
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 8;
        }

        public override void Update()
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            // delete bullets offscreen
            if (!NeonShooterGame.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
    }
}