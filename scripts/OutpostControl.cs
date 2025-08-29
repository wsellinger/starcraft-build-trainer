using Godot;
using System.Diagnostics;

namespace starcraftbuildtrainer.scripts
{
    public partial class OutpostControl : Control
    {
        [Signal] public delegate void MineralsMinedEventHandler(double value);
        [Signal] public delegate void GasMinedEventHandler(double value);

        //Nodes

        private Label _workersLabel;
        private Label _workerQueueLabel;
        private Button _townhallButton; //TODO move townhall logic into new Building scene
        private ProgressBar _workerProgressBar;

        private const string WORKERS_LABEL_NAME = "WorkersLabel";
        private const string WORKER_QUEUE_LABEL_NAME = "WorkerQueueLabel";
        private const string TOWNHALL_BUTTON_NAME = "TownhallButton";
        private const string WORKER_PROGRESS_BAR_NAME = "WorkerProgressBar";

        //References

        public IPaymentProcessor PaymentProcessor { private get; set; }

        //Data

        private int _mineralWorkersCount;
        private int _gasWorkersCount;

        //Build Worker

        private double _workerBuildProgress;
        private int _workersInBuildQueue;

        //Const

        private const double WORKER_MINERALS_PER_SECOND = 1.0;
        private const double WORKER_GAS_PER_SECOND = 1.0;
        private const int INITIAL_WORKERS = 12;

        private const int WORKER_BUILD_TIME = 12;
        private const int MAX_UNITS_IN_QUEUE = 5;

        private readonly ResourceCost _workerResourceCost = new(50, 0);

        public override void _Ready()
        {
            _workersLabel = GetNode<Label>(WORKERS_LABEL_NAME);
            _workerQueueLabel = GetNode<Label>(WORKER_QUEUE_LABEL_NAME);
            _townhallButton = GetNode<Button>(TOWNHALL_BUTTON_NAME);
            _workerProgressBar = GetNode<ProgressBar>(WORKER_PROGRESS_BAR_NAME);

            //Callbacks
            _townhallButton.Pressed += OnWorkerButtonPressed;
        }

        public override void _Process(double delta)
        {
            //Update Economy
            if (_mineralWorkersCount > 0)
                EmitSignal(SignalName.MineralsMined, _mineralWorkersCount * WORKER_MINERALS_PER_SECOND * delta);

            if (_gasWorkersCount > 0)
                EmitSignal(SignalName.GasMined, _gasWorkersCount * WORKER_GAS_PER_SECOND * delta);

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
        }

        public void Init()
        {
            Assert.That(PaymentProcessor is not null, "Payment Processor Uninitialized");

            _mineralWorkersCount = INITIAL_WORKERS;

            _workerBuildProgress = 0;
            _workersInBuildQueue = 0;

            _workerProgressBar.Hide();
            _workerQueueLabel.Hide();
        }

        private void OnWorkerButtonPressed()
        {
            if (_workersInBuildQueue >= MAX_UNITS_IN_QUEUE)
            {
                //TODO Display Error
                return;
            }

            if (PaymentProcessor.MakePayment(_workerResourceCost))
            {
                _workersInBuildQueue++;
                _workerProgressBar.Show();
                _workerQueueLabel.Show();
            }
        }
    }
}