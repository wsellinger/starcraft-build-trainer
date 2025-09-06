using Godot;

namespace starcraftbuildtrainer.scripts
{
    public partial class WorkerActivityControl : Control
    {
        //Signals

        [Signal] public delegate void MenuOpenedEventHandler(WorkerActivityControl control);
        [Signal] public delegate void ActivityCompleteEventHandler(double value);

        //Properties

        public int WorkerCount
        {
            get { return _workerCount; }
            set
            {
                _workerCount = value;
                _workerCountLabel.Text = _workerCount.ToString();
            }
        }

        //Nodes

        private TextureRect _activityTextureRect;
        private Label _workerCountLabel;
        private ProductionButton _workerButton;
        private VBoxContainer _workerCommandCard;
        private Button _basicBuildingButton;
        private Button _advancedBuildingButton;

        private const string ACTIVITY_TEXTURE_BOX_NAME = "ActivityTextureBox";
        private const string ACTIVITY_TEXTURE_NAME = "ActivityTexture";
        private const string WORKER_COUNT_LABEL_BOX_NAME = "WorkerCountLabelBox";
        private const string WORKER_COUNT_LABEL_NAME = "WorkerCountLabel";
        private const string WORKER_BUTTON_BOX_NAME = "WorkerButtonBox";
        private const string WORKER_BUTTON_NAME = "WorkerButton";
        private const string WORKER_COMMAND_CARD_NAME = "WorkerCommandCard";
        private const string BASIC_BUILDING_BUTTON_NAME = "BasicBuildingButton";
        private const string ADVANCED_BUILDING_BUTTON_NAME = "AdvancedBuildingButton";

        //Data

        private int _workerCount;

        //TODO implement construction buttons
        //TODO implement construction of supply depot
        //TODO implement supply
        //TODO implement construction of refinery
        //TODO implement gotoMinerals and gotoGas on the appropriate controls

        public override void _Ready()
        {
            _activityTextureRect = GetNode<BoxContainer>(ACTIVITY_TEXTURE_BOX_NAME).GetNode<TextureRect>(ACTIVITY_TEXTURE_NAME);
            _workerCountLabel = GetNode<BoxContainer>(WORKER_COUNT_LABEL_BOX_NAME).GetNode<Label>(WORKER_COUNT_LABEL_NAME);
            _workerButton = GetNode<BoxContainer>(WORKER_BUTTON_BOX_NAME).GetNode<ProductionButton>(WORKER_BUTTON_NAME);
            _workerCommandCard = GetNode<VBoxContainer>(WORKER_COMMAND_CARD_NAME);
            _basicBuildingButton = _workerCommandCard.GetNode<Button>(BASIC_BUILDING_BUTTON_NAME);
            _advancedBuildingButton = _workerCommandCard.GetNode<Button>(ADVANCED_BUILDING_BUTTON_NAME);

            _workerButton.Pressed += OnWorkerButtonPressed;

            _workerCommandCard.Visible = false;

        }

        public override void _Process(double delta)
        {
            
        }

        public void LoadActivityTexture(string path) => _activityTextureRect.Texture = GD.Load<Texture2D>(path);

        public void CloseMenu() => _workerCommandCard.Visible = false;

        private void OnWorkerButtonPressed()
        {
            bool isVisible = !_workerCommandCard.Visible;
            _workerCommandCard.Visible = isVisible;

            if (isVisible)
            {
                EmitSignal(SignalName.MenuOpened, this);
            }
        }
    }
}