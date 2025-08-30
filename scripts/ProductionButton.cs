using Godot;

namespace starcraftbuildtrainer.scripts
{
    public partial class ProductionButton : Button
    {
        private const float X_BUFFER = 10;
        private const string UNIT_TEXTURE_PATH = "res://assets/art/terran/units/scv.png";

        public ProductionButton(Control selectionButton)
        {
            //Texture
            var textureRect = new TextureRect()
            {
                Texture = GD.Load<Texture2D>(UNIT_TEXTURE_PATH),
                ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional,
            };

            //Box
            var box = new BoxContainer();
            box.SetAnchorsPreset(LayoutPreset.FullRect);
            box.AddChild(textureRect);

            //Button
            float x = selectionButton.Position.X + selectionButton.Size.X + X_BUFFER;
            float size = selectionButton.Size.Y / 3;
            
            Position = new Vector2(x, selectionButton.Position.Y);
            Size = new Vector2(size, size);
            Visible = false;

            AddChild(box);
        }
    }
}
