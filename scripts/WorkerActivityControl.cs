using Godot;
using System.IO;

namespace starcraftbuildtrainer.scripts
{
    public record WorkerActivityControlData(string ActivityTexturePath, ProductionButtonData[] CommandButtonData);
    public static class WorkerActivityTypes
    {
        //Paths

        private const string CONSTRUCTION_TEXTURE_PATH = "res://assets/art/terran/menu/workerBuildingIcon.png";
        private const string MINERAL_TEXTURE_PATH = "res://assets/art/shared/mineralFields.png";
        private const string GAS_TEXTURE_PATH = "res://assets/art/shared/vespeneGeyser.png";        
        private const string BASIC_BUILDING_TEXTURE_PATH = "res://assets/art/terran/menu/basicBuilding.png";

        //Buttons

        private static readonly ProductionButtonData BasicBuildingButtonData = new("BasicBuilding", BASIC_BUILDING_TEXTURE_PATH);

        //Controls

        public static readonly WorkerActivityControlData Construction = new(CONSTRUCTION_TEXTURE_PATH, []);
        public static readonly WorkerActivityControlData Minerals = new(MINERAL_TEXTURE_PATH, [BasicBuildingButtonData]);
        public static readonly WorkerActivityControlData Gas = new(GAS_TEXTURE_PATH, [BasicBuildingButtonData]);
    }

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
        private GridContainer _workerCommandCard;
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
        private bool _isMenuOpen;
        private Vector2 _buttonSize;

        //TODO implement construction buttons
        //TODO implement construction of supply depot
        //TODO implement supply
        //TODO implement construction of refinery
        //TODO implement gotoMinerals and gotoGas on the appropriate controls

        public override void _Ready()
        {
            var workerButtonBox = GetNode<BoxContainer>(WORKER_BUTTON_BOX_NAME);
            _buttonSize = workerButtonBox.Size;

            _activityTextureRect = GetNode<BoxContainer>(ACTIVITY_TEXTURE_BOX_NAME).GetNode<TextureRect>(ACTIVITY_TEXTURE_NAME);
            _workerCountLabel = GetNode<BoxContainer>(WORKER_COUNT_LABEL_BOX_NAME).GetNode<Label>(WORKER_COUNT_LABEL_NAME);
            _workerButton = workerButtonBox.GetNode<ProductionButton>(WORKER_BUTTON_NAME);
            _workerCommandCard = GetNode<GridContainer>(WORKER_COMMAND_CARD_NAME);
            _basicBuildingButton = _workerCommandCard.GetNode<Button>(BASIC_BUILDING_BUTTON_NAME);
            _advancedBuildingButton = _workerCommandCard.GetNode<Button>(ADVANCED_BUILDING_BUTTON_NAME);

            _workerButton.Pressed += OnWorkerButtonPressed;

            _workerCommandCard.Visible = false;
        }

        public void Init(WorkerActivityControlData data)
        {
            _activityTextureRect.Texture = GD.Load<Texture2D>(data.ActivityTexturePath);

            foreach (var buttonData in data.CommandButtonData)
            {
                _workerCommandCard.AddChild(ProductionButton.Instantiate(buttonData, _buttonSize));
            }
        }

        public void OpenMenu()
        {
            _isMenuOpen = true;
            _workerCommandCard.Visible = true;
            EmitSignal(SignalName.MenuOpened, this);
        }

        public void CloseMenu()
        {
            _isMenuOpen = false;
            _workerCommandCard.Visible = false;
        }

        private void OnWorkerButtonPressed()
        {
            if (!_isMenuOpen)
                OpenMenu();
            else
                CloseMenu();
        }
    }
}