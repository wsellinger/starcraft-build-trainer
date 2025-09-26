using Godot;

namespace starcraftbuildtrainer.extensions
{
    public static class CanvasItemExtensions
    {
        /// <summary>
        /// Moves this node to the back of its parent's draw order.
        /// </summary>
        public static void MoveToBack(this Control node) => node.GetParent()?.MoveChild(node, 0);
    }
}