using Godot;

namespace starcraftbuildtrainer.scripts
{
    public partial class OutpostControl : Control
    {
        //Signals

        [Signal] public delegate void MineralsMinedEventHandler(double value);
        [Signal] public delegate void GasMinedEventHandler(double value);

        //References

        public IPaymentProcessor PaymentProcessor { private get; set; }

        //Nodes

        private Label _workersLabel;
        private ProductionBuildingControl _townhallControl;

        private const string WORKERS_LABEL_NAME = "WorkersLabel";
        private const string TOWNHALL_CONTROL_NAME = "TownHallControl";

        //Data

        private int _mineralWorkersCount;
        private int _gasWorkersCount;

        //Const

        private const double WORKER_MINERALS_PER_SECOND = 1.0;
        private const double WORKER_GAS_PER_SECOND = 1.0;
        private const int INITIAL_WORKERS = 12;

        public override void _Ready()
        {
            _workersLabel = GetNode<Label>(WORKERS_LABEL_NAME);
            _townhallControl = GetNode<ProductionBuildingControl>(TOWNHALL_CONTROL_NAME);

            //Callbacks
            _townhallControl.UnitProduced += OnWorkerProduced;
        }

        public override void _Process(double delta)
        {
            //Update Economy
            if (_mineralWorkersCount > 0)
                EmitSignal(SignalName.MineralsMined, _mineralWorkersCount * WORKER_MINERALS_PER_SECOND * delta);

            if (_gasWorkersCount > 0)
                EmitSignal(SignalName.GasMined, _gasWorkersCount * WORKER_GAS_PER_SECOND * delta);

            //Update Labels
            _workersLabel.Text = _mineralWorkersCount.ToString();
        }

        public void Init()
        {
            Assert.That(PaymentProcessor is not null, "Payment Processor Uninitialized");

            _mineralWorkersCount = INITIAL_WORKERS;

            _townhallControl.PaymentProcessor = PaymentProcessor;
            _townhallControl.Init();
        }

        private void OnWorkerProduced()
        {
            _mineralWorkersCount++;
        }
    }
}