using Godot;

namespace starcraftbuildtrainer.scripts
{
	public partial class Main : CanvasLayer
	{
        //Nodes

        private OutpostControl _outpostControl;
        private ResourceControl _resourceControl;
        private ObjectiveControl _objectiveControl;
        private GameOverControl _gameOverControl;
        private MessageControl _messageControl;

        private const string OUTPOST_CONTROL_NAME = "OutpostControl";
        private const string OBJECTIVE_CONTROL_NAME = "ObjectiveControl";
        private const string RESOURCE_CONTROL_NAME = "ResourceControl";
        private const string GAME_OVER_CONTROL_NAME = "GameOverControl";
        private const string MESSAGE_CONTROL_NAME = "MessageControl";

        public override void _Ready()
        {
            //Nodes
            _outpostControl = GetNode<OutpostControl>(OUTPOST_CONTROL_NAME);
            _resourceControl = GetNode<ResourceControl>(RESOURCE_CONTROL_NAME);
            _objectiveControl = GetNode<ObjectiveControl>(OBJECTIVE_CONTROL_NAME);
            _gameOverControl = GetNode<GameOverControl>(GAME_OVER_CONTROL_NAME);
            _messageControl = GetNode<MessageControl>(MESSAGE_CONTROL_NAME);

            //Callbacks
            _outpostControl.MineralsMined += OnMineralsMined;
            _outpostControl.GasMined += OnGasMined;
            _outpostControl.MessageDispatched += OnMessageDispatched;
            _resourceControl.MessageDispatched += OnMessageDispatched;
            _objectiveControl.ObjectiveStateChange += OnObjectiveStateChange;
            _gameOverControl.Restart += OnRestart;

            //Initial Values
            _outpostControl.ResourceManager = _resourceControl;

            Init();
        }

        private void OnMessageDispatched(string message)
        {
            _messageControl.DisplayMessage(message);
        }

        public override void _Process(double delta)
		{
            //Early Out if Game Over
            if (_gameOverControl.IsGameOver)
                return;

            //Update Objectives
            //TODO improve this when updating to handle more complex objectives, should be on build events and amount should be stored internal to objective control for display
            _objectiveControl.CheckObjectiveComplete(_outpostControl.WorkerCount); 
        }

        private void Init()
        {
            _outpostControl.Init();
            _resourceControl.Init(OutpostControl.InitialSupply);
            _objectiveControl.Init();
            _gameOverControl.Init();
        }

        //Outpost Callbacks

        private void OnMineralsMined(double value) => _resourceControl.AddMinerals(value);
        private void OnGasMined(double value) => _resourceControl.AddGas(value);

        //Objective Callbacks

        private void OnObjectiveStateChange(ObjectiveState state) => _gameOverControl.GameOver(state);

        //GameOver Callbacks

        private void OnRestart() => Init();

    }
}
