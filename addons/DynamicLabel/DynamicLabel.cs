using Godot;

/// <summary>
/// A Label that automatically scales its font size
/// up or down to fit within its bounding rectangle.
/// </summary>
[Tool]
public partial class DynamicLabel : Label
{
    [Export] public int MinSize { get; set; } = 8;
    [Export] public int MaxSize { get; set; } = 256;
    [Export] public bool UseCustomMinSize { get; set; } = true;

    public override void _Ready()
    {
        TextOverrunBehavior = TextServer.OverrunBehavior.TrimChar;
        if (UseCustomMinSize) UpdateCustomMinSize();
        UpdateFontSize();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationResized)
            UpdateFontSize();
    }

    public override void _Process(double delta)
    {
        // Keep editor responsive when you change text/property
        if (Engine.IsEditorHint())
            UpdateFontSize();
    }

    private void UpdateFontSize()
    {
        var font = GetThemeFont("font");
        if (font == null || string.IsNullOrEmpty(Text) || Size.X <= 0 || Size.Y <= 0)
            return;

        int best = MinSize;
        int lo = MinSize, hi = MaxSize;

        // Binary search for the largest size that still fits
        while (lo <= hi)
        {
            int mid = (lo + hi) / 2;
            Vector2 textSize = font.GetStringSize(Text, HorizontalAlignment, -1, mid);

            if (textSize.X <= Size.X && textSize.Y <= Size.Y)
            {
                best = mid;
                lo = mid + 1;
            }
            else
            {
                hi = mid - 1;
            }
        }

        AddThemeFontSizeOverride("font_size", best);

        if (UseCustomMinSize)
            UpdateCustomMinSize();
    }

    private void UpdateCustomMinSize()
    {
        var font = GetThemeFont("font");
        if (font == null)
            return;

        // Make the container allow shrinking down to the minimum font size
        Vector2 minTextSize = font.GetStringSize(Text ?? "", HorizontalAlignment, -1, MinSize);
        SetCustomMinimumSize(minTextSize);
        // If you prefer the label to be able to shrink *below* min text size and just clip,
        // use: SetCustomMinimumSize(Vector2.Zero);
    }

    // Optional helper to force recalculation from other scripts
    public void ForceUpdate() => UpdateFontSize();
}
