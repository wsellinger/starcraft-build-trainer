using Godot;

namespace starcraftbuildtrainer.scripts
{
	public partial class Main : CanvasLayer
	{
        //Nodes

        private Label _mineralsLabel;
        private Label _gasLabel;
        private Label _workersLabel;
        private Label _workerQueueLabel;

        private Button _workerButton;
        private Button _restartButton;

        private Control _victoryScreen;
        private Control _defeatScreen;

        private ObjectiveControl _objectiveControl;

        private ProgressBar _workerProgressBar;

        private const string MINERALS_LABEL_NAME = "MineralsLabel";
        private const string GAS_LABEL_NAME = "GasLabel";
        private const string WORKERS_LABEL_NAME = "WorkersLabel";
        private const string WORKER_QUEUE_LABEL_NAME = "WorkerQueueLabel";

        private const string WORKER_BUTTON_NAME = "WorkerButton";
        private const string RESTART_BUTTON_NAME = "RestartButton";

        private const string VICTORY_SCREEN_NAME = "VictoryScreen";
        private const string DEFEAT_SCREEN_NAME = "DefeatScreen";

        private const string WORKER_PROGRESS_BAR_NAME = "WorkerProgressBar";

        private const string OBJECTIVE_CONTROL_NAME = "ObjectiveControl";

        //Data

        private double _mineralCount = 0;
        private double _gasCount = 0;
        private int _mineralWorkersCount = 0;
        private int _gasWorkersCount = 0;

        private bool _isGameOver = false;

        //Build Worker

        private double _workerBuildProgress = 0;
        private int _workersInBuildQueue = 0;

        //Constants

        private const double WORKER_MINERALS_PER_SECOND = 1.0;
        private const double WORKER_GAS_PER_SECOND = 1.0;
        private const int WORKER_MINERAL_COST = 50;

        private const int INITIAL_MINERALS = 50;
        private const int INITIAL_GAS = 0;
        private const int INITIAL_WORKERS = 12;

        private const int WORKER_BUILD_TIME = 12;
        private const int MAX_UNITS_IN_QUEUE = 5;

        public override void _Ready()
        {
            //Get Nodes
            _mineralsLabel = GetNode<Label>(MINERALS_LABEL_NAME);
            _gasLabel = GetNode<Label>(GAS_LABEL_NAME);
            _workersLabel = GetNode<Label>(WORKERS_LABEL_NAME);
            _workerQueueLabel = GetNode<Label>(WORKER_QUEUE_LABEL_NAME);

            _victoryScreen = GetNode<Control>(VICTORY_SCREEN_NAME);
            _defeatScreen = GetNode<Control>(DEFEAT_SCREEN_NAME);

            _workerButton = GetNode<Button>(WORKER_BUTTON_NAME);
            _restartButton = GetNode<Button>(RESTART_BUTTON_NAME);

            _workerProgressBar = GetNode<ProgressBar>(WORKER_PROGRESS_BAR_NAME);

            _objectiveControl = GetNode<ObjectiveControl>(OBJECTIVE_CONTROL_NAME);

            //Callbacks
            _workerButton.Pressed += OnWorkerButtonPressed;
            _restartButton.Pressed += OnRestartButtonPressed;

            _objectiveControl.ObjectiveComplete += OnObjectiveComplete;
            _objectiveControl.ObjectiveFailed += OnObjectiveFailed;

            //Initial Values
            Init();
        }

        public override void _Process(double delta)
		{
            //Early Out if Game Over
            if (_isGameOver)
                return;

            //Update Economy
            _mineralCount += _mineralWorkersCount * WORKER_MINERALS_PER_SECOND * delta;
            _gasCount += _gasWorkersCount * WORKER_GAS_PER_SECOND * delta;

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
            _mineralsLabel.Text = Mathf.Round(_mineralCount).ToString();
            _gasLabel.Text = Mathf.Round(_gasCount).ToString();
            _workersLabel.Text = _mineralWorkersCount.ToString();
            _workerQueueLabel.Text = _workersInBuildQueue.ToString();

            _workerProgressBar.Value = _workerBuildProgress / WORKER_BUILD_TIME;

            //Update Objectives
            _objectiveControl.CheckObjectiveComplete(_mineralCount);
        }

        private void OnWorkerButtonPressed()
        {
            if (_mineralCount >= WORKER_MINERAL_COST && 
                _workersInBuildQueue < MAX_UNITS_IN_QUEUE)
            {
                _mineralCount -= WORKER_MINERAL_COST;
                _workersInBuildQueue++;
                _workerProgressBar.Show();
                _workerQueueLabel.Show();
            }
        }

        private void OnRestartButtonPressed()
        {
            Init();
            _isGameOver = false;
        }

        private void OnObjectiveComplete() => GameOver(_victoryScreen);
        private void OnObjectiveFailed() => GameOver(_defeatScreen);

        private void GameOver(Control screen)
        {
            screen.Show();
            _restartButton.Show();
            _isGameOver = true;
        }

        private void Init()
        {
            _mineralCount = INITIAL_MINERALS;
            _gasCount = INITIAL_GAS;
            _mineralWorkersCount = INITIAL_WORKERS;

            _isGameOver = false;

            _workerBuildProgress = 0;
            _workersInBuildQueue = 0;

            _victoryScreen.Hide();
            _defeatScreen.Hide();
            _restartButton.Hide();
            _workerProgressBar.Hide();
            _workerQueueLabel.Hide();

            _objectiveControl.Init();
        }
    }
}
