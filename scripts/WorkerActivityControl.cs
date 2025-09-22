using Godot;
using System;
using System.IO;

namespace starcraftbuildtrainer.scripts
{

    public partial class WorkerActivityControl : Control
    {
        //Events

        public event Action<WorkerActivityControl> MenuOpened;
        public event Action<WorkerActivityControl, ActionButtonData> ActionSelected;

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
        private DynamicLabel _workerCountLabel;
        private ProductionButton _workerButton;
        private WorkerCommandCard _workerCommandCard;

        private const string ACTIVITY_TEXTURE_NAME = "ActivityTexture";
        private const string WORKER_COUNT_LABEL_NAME = "WorkerCountLabel";
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
            _activityTextureRect = GetNode<TextureRect>(ACTIVITY_TEXTURE_NAME);
            _workerCountLabel = GetNode<DynamicLabel>(WORKER_COUNT_LABEL_NAME); //TODO dynamic font size
            _workerButton = GetNode<ProductionButton>(WORKER_BUTTON_NAME);
            _workerCommandCard = GetNode<WorkerCommandCard>(WORKER_COMMAND_CARD_NAME);

            _workerButton.Pressed += OnWorkerButtonPressed;
            _workerCommandCard.ActionSelected += OnActionSelected;

            _workerCommandCard.ButtonSize = _workerButton.Size;
            _workerCommandCard.Visible = false;
        }

        private void OnActionSelected(ActionButtonData data)
        {
            ActionSelected.Invoke(this, data);
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
            MenuOpened.Invoke(this);
        }

        public void CloseMenu()
        {
            _isMenuOpen = false;
            _workerCommandCard.Close();
        }

        private void OnWorkerButtonPressed()
        {
            if (!_isMenuOpen)
                OpenMenu(); //TODO check if workers > 0 before opening
            else
                CloseMenu();
        }
    }
}