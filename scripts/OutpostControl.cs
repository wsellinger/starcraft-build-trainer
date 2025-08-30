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

        private ProductionBuildingControl _townhallControl;
        private ResourceNodeControl _idleControl;
        private ResourceNodeControl _mineralsControl;
        private ResourceNodeControl _gasControlA;
        private ResourceNodeControl _gasControlB;

        private const string TOWNHALL_CONTROL_NAME = "TownHallControl";
        private const string IDLE_CONTROL_NAME = "IdleControl";
        private const string MINERALS_CONTROL_NAME = "MineralsControl";
        private const string GAS_CONTROL_A_NAME = "GasControl_A";
        private const string GAS_CONTROL_B_NAME = "GasControl_B";

        //Data

        private int _idleWorkersCount;
        private int _mineralWorkersCount;
        private int _gasWorkersCount_A;
        private int _gasWorkersCount_B;

        //Const

        private const double WORKER_MINERALS_PER_SECOND = 1.0;
        private const double WORKER_GAS_PER_SECOND = 1.0;
        private const int INITIAL_WORKERS = 12;

        public override void _Ready()
        {
            _townhallControl = GetNode<ProductionBuildingControl>(TOWNHALL_CONTROL_NAME);
            _idleControl = GetNode<ResourceNodeControl>(IDLE_CONTROL_NAME);
            _mineralsControl = GetNode<ResourceNodeControl>(MINERALS_CONTROL_NAME);
            _gasControlA = GetNode<ResourceNodeControl>(GAS_CONTROL_A_NAME);
            _gasControlB = GetNode<ResourceNodeControl>(GAS_CONTROL_B_NAME);

            //Callbacks
            _townhallControl.UnitProduced += OnWorkerProduced;
        }

        public override void _Process(double delta)
        {
            //Update Economy
            if (_mineralWorkersCount > 0)
                EmitSignal(SignalName.MineralsMined, _mineralWorkersCount * WORKER_MINERALS_PER_SECOND * delta);

            if (_gasWorkersCount_A > 0)
                EmitSignal(SignalName.GasMined, _gasWorkersCount_A * WORKER_GAS_PER_SECOND * delta);

            //Update Labels
            _idleControl.Text = _idleWorkersCount.ToString();
            _mineralsControl.Text = _mineralWorkersCount.ToString();
            _gasControlA.Text = _gasWorkersCount_A.ToString();
            _gasControlB.Text = _gasWorkersCount_B.ToString();
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