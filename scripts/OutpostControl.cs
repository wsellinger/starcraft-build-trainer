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
        private WorkerActivityControl _constructionControl;
        private WorkerActivityControl _mineralsControl;
        private WorkerActivityControl _gasControlA;
        private WorkerActivityControl _gasControlB;

        private const string TOWNHALL_CONTROL_NAME = "TownHallControl";
        private const string IDLE_CONTROL_NAME = "ConstructionControl";
        private const string MINERALS_CONTROL_NAME = "MineralsControl";
        private const string GAS_CONTROL_A_NAME = "GasControl_A";
        private const string GAS_CONTROL_B_NAME = "GasControl_B";

        private const string CONSTRUCTION_TEXTURE_PATH = "res://assets/art/terran/workerBuildingIcon.png";
        private const string MINERAL_TEXTURE_PATH = "res://assets/art/shared/mineralFields.png";
        private const string GAS_TEXTURE_PATH = "res://assets/art/shared/vespeneGeyser.png";

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
            _constructionControl = GetNode<WorkerActivityControl>(IDLE_CONTROL_NAME);
            _mineralsControl = GetNode<WorkerActivityControl>(MINERALS_CONTROL_NAME);
            _gasControlA = GetNode<WorkerActivityControl>(GAS_CONTROL_A_NAME);
            _gasControlB = GetNode<WorkerActivityControl>(GAS_CONTROL_B_NAME);

            _constructionControl.LoadActivityTexture(CONSTRUCTION_TEXTURE_PATH);
            _mineralsControl.LoadActivityTexture(MINERAL_TEXTURE_PATH);
            _gasControlA.LoadActivityTexture(GAS_TEXTURE_PATH);
            _gasControlB.LoadActivityTexture(GAS_TEXTURE_PATH);

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
            _constructionControl.Text = _idleWorkersCount.ToString();
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