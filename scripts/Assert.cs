using Godot;
using System.Diagnostics;

namespace starcraftbuildtrainer.scripts
{
    public static class Assert
    {
        public static void That(bool condition, string message = null)
        {
#if DEBUG
            if (!condition)
            {
                if (message is not null)
                    GD.PushError("Assertion Failed:", message);
                else
                    GD.PushError("Assertion Failed!");

                Debugger.Log(0, "Assert", $"Assertion Failed: {message}\n");
                Debugger.Break();
            }
#endif
        }
    }
}