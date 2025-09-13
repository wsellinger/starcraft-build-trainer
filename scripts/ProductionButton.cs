using Godot;

namespace starcraftbuildtrainer.scripts
{
    public partial class ProductionButton : Button
    {
        //Static

        private static readonly PackedScene _scene = GD.Load<PackedScene>("res://scenes/production_button.tscn");

        public static ProductionButton Instantiate() => _scene.Instantiate<ProductionButton>();

        public static ProductionButton Instantiate(ProductionButtonData data, Vector2 size)
        {
            var button = _scene.Instantiate<ProductionButton>();
            button.Type = data.Type;
            button.Name = data.Type.ToString();
            button.TexturePath = data.TexturePath;
            button.CustomMinimumSize = size;
            return button;
        }

        //Instanced

        [Signal] public delegate void SelectedEventHandler(ProductionButton button);

        public ProductionButtonType Type { get; set; }

        public override void _Ready()
        {
            Pressed += OnPressed;
        }

        private void OnPressed()
        {
            EmitSignal(SignalName.Selected, this);
        }

        public string TexturePath 
        { 
            set => GetNode<BoxContainer>(nameof(BoxContainer)).
                       GetNode<TextureRect>(nameof(TextureRect)).
                           Texture = GD.Load<Texture2D>(value); 
        }
    }
}
