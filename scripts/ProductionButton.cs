using Godot;

namespace starcraftbuildtrainer.scripts
{
    public partial class ProductionButton : Button
    {
        public string TexturePath 
        { 
            set => GetNode<BoxContainer>(nameof(BoxContainer)).
                       GetNode<TextureRect>(nameof(TextureRect)).
                           Texture = GD.Load<Texture2D>(value); 
        }

        public override void _Ready()
        {
        }
    }
}
