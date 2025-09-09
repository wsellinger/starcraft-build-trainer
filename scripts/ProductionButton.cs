using Godot;

namespace starcraftbuildtrainer.scripts
{
    public partial class ProductionButton : Button
    {
        private static readonly PackedScene _scene = GD.Load<PackedScene>("res://scenes/production_button.tscn");

        public static ProductionButton Instantiate() => _scene.Instantiate<ProductionButton>();

        public static ProductionButton Instantiate(ProductionButtonData data, Vector2 size)
        {
            var button = _scene.Instantiate<ProductionButton>();
            button.Name = data.Type.ToString();
            button.TexturePath = data.TexturePath;
            button.CustomMinimumSize = size;
            return button;
        }


        public string TexturePath 
        { 
            set => GetNode<BoxContainer>(nameof(BoxContainer)).
                       GetNode<TextureRect>(nameof(TextureRect)).
                           Texture = GD.Load<Texture2D>(value); 
        }
    }
}
