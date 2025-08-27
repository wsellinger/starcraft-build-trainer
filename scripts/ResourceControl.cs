using Godot;

namespace starcraftbuildtrainer.scripts
{
    public partial class ResourceControl : Control
    {
        public double MineralCount { get; set; }
        public double GasCount { get; set; }

        //Nodes

        private Label _mineralsLabel;
        private Label _gasLabel;

        private const string MINERALS_LABEL_NAME = "MineralsLabel";
        private const string GAS_LABEL_NAME = "GasLabel";

        //Defaults

        private const int INITIAL_MINERALS = 50;
        private const int INITIAL_GAS = 0;

        public override void _Ready()
        {
            _mineralsLabel = GetNode<Label>(MINERALS_LABEL_NAME);
            _gasLabel = GetNode<Label>(GAS_LABEL_NAME);
        }

        public override void _Process(double delta)
        {
            _mineralsLabel.Text = Mathf.Round(MineralCount).ToString();
            _gasLabel.Text = Mathf.Round(GasCount).ToString();
        }

        public void Init()
        {
            MineralCount = INITIAL_MINERALS;
            GasCount = INITIAL_GAS;
        }
    }
}

