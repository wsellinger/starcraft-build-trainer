using Godot;

namespace starcraftbuildtrainer.scripts
{
    public partial class ProductionButton : Button
    {
        public string TexturePath { private get;  set; }

        public override void _Ready()
        {
            Assert.That(TexturePath is not null and not "");
            GetNode<BoxContainer>(nameof(BoxContainer)).
                GetNode<TextureRect>(nameof(TextureRect)).
                    Texture = GD.Load<Texture2D>(TexturePath);
        }

    }
}
