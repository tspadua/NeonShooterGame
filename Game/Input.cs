using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Linq;

namespace NeonShooter
{

    static class Input
    {
        ////////////////////////////////////////////////////////////////////////////
        //                                                                        //
        //  Note: pushing a thumbstick forward will return a positive y value.    //
        //  In screen coordinates, y values increase going downwards.             //
        //  We want to invert the y axis on the controller so that pushing        //
        //  the thumbstick up will aim or move us towards the top of the screen.  //
        //                                                                        //
        ////////////////////////////////////////////////////////////////////////////

        private static KeyboardState keyboardState, lastKeyboardState;
        private static MouseState mouseState, lastMouseState;
        private static GamePadState gamepadState, lastGamepadState;
        private static bool isAimingWithMouse = false;

        public static Vector2 MousePosition { get { return new Vector2(mouseState.X, mouseState.Y); } }

        public static void Update()
        {
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
            lastGamepadState = gamepadState;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);


            // If the player pressed one of the arrow keys or is using a gamepad to aim, we want to disable mouse aiming.
            if (new[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down }.Any(x => keyboardState.IsKeyDown(x)) || gamepadState.ThumbSticks.Right != Vector2.Zero)
                isAimingWithMouse = false;
            // Otherwise, if the player moves the mouse, enable mouse aiming.
            else if (MousePosition != new Vector2(lastMouseState.X, lastMouseState.Y))
                isAimingWithMouse = true;
        }

        // Checks if a key was just pressed down
        public static bool WasKeyPressed(Keys key)
        {
            return lastKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);
        }
        // Checks if a gamepad button was just pressed down
        public static bool WasButtonPressed(Buttons button)
        {
            return lastGamepadState.IsButtonUp(button) && gamepadState.IsButtonDown(button);
        }

        public static Vector2 GetMovementDirection()
        {
            Vector2 direction = gamepadState.ThumbSticks.Left; // left thumbstick determines direction
            direction.Y *= -1; // Thumbstick direction is inverted

            if (keyboardState.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D))
                direction.X += 1;
            if (keyboardState.IsKeyDown(Keys.W))
                direction.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S))
                direction.Y += 1;

            // Clamp the length of the vector to a maximum of 1.
            // LengthSquared() is a small performance optimization;
            // computing the square of the length is a bit faster than computing the length itself
            // because it avoids the relatively slow square root operation.
            if (direction.LengthSquared() > 1)
                direction.Normalize();

            return direction;
        }

        public static Vector2 GetAimDirection()
        {
            if (isAimingWithMouse)
                return GetMouseAimDirection();

            // Gamepad aim is determined by the right thumbstick
            Vector2 direction = gamepadState.ThumbSticks.Right;
            direction.Y *= -1;

            if (keyboardState.IsKeyDown(Keys.Left))
                direction.X -= 1;
            if (keyboardState.IsKeyDown(Keys.Right))
                direction.X += 1;
            if (keyboardState.IsKeyDown(Keys.Up))
                direction.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.Down))
                direction.Y += 1;

            // If there's no aim input, return zero. Otherwise normalize the direction to have a length of 1.
            if (direction == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(direction);
        }

        public static Vector2 GetMouseAimDirection()
        {
            Vector2 direction = MousePosition - PlayerShip.Instance.Position;

            if (direction == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(direction);
        }

        public static bool wasBombButtonPressed()
        {
            return WasButtonPressed(Buttons.LeftTrigger) || WasButtonPressed(Buttons.RightTrigger) || WasKeyPressed(Keys.Space);
        }

    }

}
