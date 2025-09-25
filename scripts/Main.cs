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

        private const string OUTPOST_CONTROL_NAME = "OutpostControl";
        private const string OBJECTIVE_CONTROL_NAME = "ObjectiveControl";
        private const string RESOURCE_CONTROL_NAME = "ResourceControl";
        private const string GAME_OVER_CONTROL_NAME = "GameOverControl";

        public override void _Ready()
        {
            //Nodes
            _outpostControl = GetNode<OutpostControl>(OUTPOST_CONTROL_NAME);
            _resourceControl = GetNode<ResourceControl>(RESOURCE_CONTROL_NAME);
            _objectiveControl = GetNode<ObjectiveControl>(OBJECTIVE_CONTROL_NAME);
            _gameOverControl = GetNode<GameOverControl>(GAME_OVER_CONTROL_NAME);

            //Callbacks
            _outpostControl.MineralsMined += OnMineralsMined;
            _outpostControl.GasMined += OnGasMined;
            _objectiveControl.ObjectiveStateChange += OnObjectiveStateChange;
            _gameOverControl.Restart += OnRestart;

            //Initial Values
            _outpostControl.ResourceManager = _resourceControl;

            Init();
        }

        public override void _Process(double delta)
		{
            //Early Out if Game Over
            if (_gameOverControl.IsGameOver)
                return;

            //Update Objectives
            _objectiveControl.CheckObjectiveComplete(_resourceControl.Minerals);
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
