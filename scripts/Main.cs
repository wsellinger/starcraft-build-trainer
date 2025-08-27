using Godot;

namespace starcraftbuildtrainer.scripts
{
	public partial class Main : CanvasLayer
	{
        //Nodes

        private Label _workersLabel;
        private Label _workerQueueLabel;

        private Button _workerButton;

        private ResourceControl _resourceControl;
        private ObjectiveControl _objectiveControl;
        private GameOverControl _gameOverControl;

        private ProgressBar _workerProgressBar;

        private const string WORKERS_LABEL_NAME = "WorkersLabel";
        private const string WORKER_QUEUE_LABEL_NAME = "WorkerQueueLabel";

        private const string WORKER_BUTTON_NAME = "WorkerButton";

        private const string WORKER_PROGRESS_BAR_NAME = "WorkerProgressBar";

        private const string OBJECTIVE_CONTROL_NAME = "ObjectiveControl";
        private const string RESOURCE_CONTROL_NAME = "ResourceControl";
        private const string GAME_OVER_CONTROL_NAME = "GameOverControl";

        //Data

        private int _mineralWorkersCount;
        private int _gasWorkersCount;

        //Build Worker

        private double _workerBuildProgress;
        private int _workersInBuildQueue;

        //Constants

        private const double WORKER_MINERALS_PER_SECOND = 1.0;
        private const double WORKER_GAS_PER_SECOND = 1.0;
        private const int WORKER_MINERAL_COST = 50;
        private const int INITIAL_WORKERS = 12;

        private const int WORKER_BUILD_TIME = 12;
        private const int MAX_UNITS_IN_QUEUE = 5;

        public override void _Ready()
        {
            //Get Nodes
            _workersLabel = GetNode<Label>(WORKERS_LABEL_NAME);
            _workerQueueLabel = GetNode<Label>(WORKER_QUEUE_LABEL_NAME);

            _workerButton = GetNode<Button>(WORKER_BUTTON_NAME);

            _workerProgressBar = GetNode<ProgressBar>(WORKER_PROGRESS_BAR_NAME);

            _resourceControl = GetNode<ResourceControl>(RESOURCE_CONTROL_NAME);
            _objectiveControl = GetNode<ObjectiveControl>(OBJECTIVE_CONTROL_NAME);
            _gameOverControl = GetNode<GameOverControl>(GAME_OVER_CONTROL_NAME);

            //Callbacks
            _workerButton.Pressed += OnWorkerButtonPressed;

            _objectiveControl.ObjectiveStateChange += OnObjectiveStateChange;
            _gameOverControl.Restart += OnRestart;

            //Initial Values
            Init();
        }

        public override void _Process(double delta)
		{
            //Early Out if Game Over
            if (_gameOverControl.IsGameOver)
                return;

            //Update Economy
            _resourceControl.MineralCount += _mineralWorkersCount * WORKER_MINERALS_PER_SECOND * delta;
            _resourceControl.GasCount += _gasWorkersCount * WORKER_GAS_PER_SECOND * delta;

            //Update Production
            if (_workersInBuildQueue > 0)
            {
                _workerBuildProgress += delta;

                if (_workerBuildProgress >= WORKER_BUILD_TIME)
                {
                    _workersInBuildQueue--;
                    _mineralWorkersCount++;
                    _workerBuildProgress = 0;

                    if (_workersInBuildQueue == 0)
                    {
                        _workerProgressBar.Hide();
                        _workerQueueLabel.Hide();
                    }
                }
            }

            //Update Labels
            _workersLabel.Text = _mineralWorkersCount.ToString();
            _workerQueueLabel.Text = _workersInBuildQueue.ToString();

            _workerProgressBar.Value = _workerBuildProgress / WORKER_BUILD_TIME;

            //Update Objectives
            _objectiveControl.CheckObjectiveComplete(_resourceControl.MineralCount);
        }

        private void Init()
        {
            _mineralWorkersCount = INITIAL_WORKERS;

            _workerBuildProgress = 0;
            _workersInBuildQueue = 0;

            _workerProgressBar.Hide();
            _workerQueueLabel.Hide();

            _resourceControl.Init();
            _objectiveControl.Init();
        }

        private void OnWorkerButtonPressed()
        {
            if (_resourceControl.MineralCount >= WORKER_MINERAL_COST && 
                _workersInBuildQueue < MAX_UNITS_IN_QUEUE)
            {
                _resourceControl.MineralCount -= WORKER_MINERAL_COST;
                _workersInBuildQueue++;
                _workerProgressBar.Show();
                _workerQueueLabel.Show();
            }
        }

        private void OnRestart() => Init();

        private void OnObjectiveStateChange(ObjectiveState state) => _gameOverControl.GameOver(state);
    }
}
