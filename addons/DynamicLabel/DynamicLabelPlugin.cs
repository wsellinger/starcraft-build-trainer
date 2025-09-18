using Godot;

[Tool]
public partial class DynamicLabelPlugin : EditorPlugin
{
    public override void _EnterTree()
    {
        var script = GD.Load<Script>("res://addons/DynamicLabel/DynamicLabel.cs");
        var icon = GD.Load<Texture2D>("res://addons/DynamicLabel/icon.svg"); // Replace with your own icon
        AddCustomType("DynamicLabel", "Label", script, icon);
    }

    public override void _ExitTree()
    {
        RemoveCustomType("DynamicLabel");
    }
}
