using Godot;
using System;
using System.IO;

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
        private WorkerCommandCard _workerCommandCard;

        private const string ACTIVITY_TEXTURE_BOX_NAME = "ActivityTextureBox";
        private const string ACTIVITY_TEXTURE_NAME = "ActivityTexture";
        private const string WORKER_COUNT_LABEL_BOX_NAME = "WorkerCountLabelBox";
        private const string WORKER_COUNT_LABEL_NAME = "WorkerCountLabel";
        private const string WORKER_BUTTON_BOX_NAME = "WorkerButtonBox";
        private const string WORKER_BUTTON_NAME = "WorkerButton";
        private const string WORKER_COMMAND_CARD_NAME = "WorkerCommandCard";

        //Data

        private int _workerCount;
        private bool _isMenuOpen;

        //TODO implement construction of supply depot
        //TODO implement supply
        //TODO implement construction of refinery
        //TODO implement gotoMinerals and gotoGas on the appropriate controls

        public override void _Ready()
        {
            _activityTextureRect = GetNode<BoxContainer>(ACTIVITY_TEXTURE_BOX_NAME).GetNode<TextureRect>(ACTIVITY_TEXTURE_NAME);
            _workerCountLabel = GetNode<BoxContainer>(WORKER_COUNT_LABEL_BOX_NAME).GetNode<Label>(WORKER_COUNT_LABEL_NAME);
            
            var workerButtonBox = GetNode<BoxContainer>(WORKER_BUTTON_BOX_NAME);
            _workerButton = workerButtonBox.GetNode<ProductionButton>(WORKER_BUTTON_NAME);
            _workerCommandCard = GetNode<WorkerCommandCard>(WORKER_COMMAND_CARD_NAME);

            _workerButton.Pressed += OnWorkerButtonPressed;

            _workerCommandCard.ButtonSize = workerButtonBox.Size;
            _workerCommandCard.Visible = false;
        }

        public void Init(WorkerActivityControlData data)
        {
            _activityTextureRect.Texture = GD.Load<Texture2D>(data.ActivityTexturePath);
            _workerCommandCard.Init(data);
        }

        public void OpenMenu()
        {
            _isMenuOpen = true;
            _workerCommandCard.Open();
            EmitSignal(SignalName.MenuOpened, this);
        }

        public void CloseMenu()
        {
            _isMenuOpen = false;
            _workerCommandCard.Close();
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