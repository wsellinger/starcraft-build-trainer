using Godot;
using System;

namespace starcraftbuildtrainer.scripts
{
	public partial class Main : CanvasLayer
	{
        //Nodes

        private Label _mineralsLabel;
        private Label _gasLabel;
        private Label _workersLabel;
        private Label _objectiveLabel;
        private Label _timerLabel;
        private Label _workerQueueLabel;

        private Button _workerButton;
        private Button _restartButton;

        private Control _victoryScreen;
        private Control _defeatScreen;

        private ProgressBar _workerProgressBar;

        private const string MINERALS_LABEL_NAME = "MineralsLabel";
        private const string GAS_LABEL_NAME = "GasLabel";
        private const string WORKERS_LABEL_NAME = "WorkersLabel";
        private const string OBJECTIVE_LABEL_NAME = "ObjectiveLabel";
        private const string TIMER_LABEL_NAME = "TimerLabel";
        private const string WORKER_QUEUE_LABEL_NAME = "WorkerQueueLabel";

        private const string WORKER_BUTTON_NAME = "WorkerButton";
        private const string RESTART_BUTTON_NAME = "RestartButton";

        private const string VICTORY_SCREEN_NAME = "VictoryScreen";
        private const string DEFEAT_SCREEN_NAME = "DefeatScreen";

        private const string WORKER_PROGRESS_BAR_NAME = "WorkerProgressBar";

        //Data

        private double _mineralCount = 0;
        private double _gasCount = 0;
        private int _mineralWorkersCount = 0;
        private int _gasWorkersCount = 0;

        //Build Worker

        private double _workerBuildProgress = 0;
        private int _workersInBuildQueue = 0;

        //Objective

        private double _timerSeconds = 0;
        private bool _isGameOver = false;

        //Constants

        private const double WORKER_MINERALS_PER_SECOND = 1.0;
        private const double WORKER_GAS_PER_SECOND = 1.0;
        private const int WORKER_MINERAL_COST = 50;

        private const int INITIAL_MINERALS = 50;
        private const int INITIAL_GAS = 0;
        private const int INITIAL_WORKERS = 12;

        private const int WORKER_BUILD_TIME = 12;
        private const int MAX_UNITS_IN_QUEUE = 5;

        private readonly TimeSpan INITIAL_TIMER_SPAN = new(0, 1, 0);
        private const string TIMER_FORMAT = @"mm\:ss";

        private const int MINERAL_WIN_COUNT = 750;
        private const string OBJECTIVE_FORMAT = "Mine {0} Minerals";

        public override void _Ready()
        {
            //Get Nodes
            _mineralsLabel = GetNode<Label>(MINERALS_LABEL_NAME);
            _gasLabel = GetNode<Label>(GAS_LABEL_NAME);
            _workersLabel = GetNode<Label>(WORKERS_LABEL_NAME);
            _objectiveLabel = GetNode<Label>(OBJECTIVE_LABEL_NAME);
            _timerLabel = GetNode<Label>(TIMER_LABEL_NAME);
            _workerQueueLabel = GetNode<Label>(WORKER_QUEUE_LABEL_NAME);

            _victoryScreen = GetNode<Control>(VICTORY_SCREEN_NAME);
            _defeatScreen = GetNode<Control>(DEFEAT_SCREEN_NAME);

            _workerButton = GetNode<Button>(WORKER_BUTTON_NAME);
            _restartButton = GetNode<Button>(RESTART_BUTTON_NAME);

            _workerProgressBar = GetNode<ProgressBar>(WORKER_PROGRESS_BAR_NAME);

            //Callbacks
            _workerButton.Pressed += OnWorkerButtonPressed;
            _restartButton.Pressed += OnRestartButtonPressed;

            //Initial Values
            Init();
        }

        public override void _Process(double delta)
		{
            //Early Out if Game Over
            if (_isGameOver)
                return;

            //Update Timer
            _timerSeconds -= delta;

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
            _timerLabel.Text = TimeSpan.FromSeconds(_timerSeconds).ToString(TIMER_FORMAT);
            _workerQueueLabel.Text = _workersInBuildQueue.ToString();

            _workerProgressBar.Value = _workerBuildProgress / WORKER_BUILD_TIME;

            //Update Objectives
            if (_mineralCount >= MINERAL_WIN_COUNT)
            {
                GameOver(_victoryScreen);
            }
            else if (_timerSeconds <= 0)
            {
                GameOver(_defeatScreen);
            }

            //Local

            void GameOver(Control screen)
            {
                screen.Show();
                _restartButton.Show();
                _isGameOver = true;
            }
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

        private void Init()
        {
            _mineralCount = INITIAL_MINERALS;
            _gasCount = INITIAL_GAS;
            _mineralWorkersCount = INITIAL_WORKERS;
            _objectiveLabel.Text = string.Format(OBJECTIVE_FORMAT, MINERAL_WIN_COUNT);
            
            _workerBuildProgress = 0;
            _workersInBuildQueue = 0;

            _timerSeconds = INITIAL_TIMER_SPAN.TotalSeconds;
            _isGameOver = false;

            _victoryScreen.Hide();
            _defeatScreen.Hide();
            _restartButton.Hide();
            _workerProgressBar.Hide();
            _workerQueueLabel.Hide();
        }
    }
}
