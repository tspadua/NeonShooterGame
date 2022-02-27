using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace NeonShooter
{

    static class Art
    {
        public static Texture2D Player { get; private set; }
        public static Texture2D Seeker { get; private set; }
        public static Texture2D Wanderer { get; private set; }
        public static Texture2D Bullet { get; private set; }
        public static Texture2D Pointer { get; private set; }
        public static SpriteFont UiFont { get; private set; }
        public static void Load(ContentManager content)
        {
            Player = content.Load<Texture2D>("Sprites/Player");
            Seeker = content.Load<Texture2D>("Sprites/Seeker");
            Wanderer = content.Load<Texture2D>("Sprites/Wanderer");
            Bullet = content.Load<Texture2D>("Sprites/Bullet");
            Pointer = content.Load<Texture2D>("Sprites/Pointer");

            UiFont = content.Load<SpriteFont>("Fonts/NovaSquare");
        }
    }

}