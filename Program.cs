using System;

namespace NeonShooter
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new NeonShooterGame())
                game.Run();
        }
    }
}
