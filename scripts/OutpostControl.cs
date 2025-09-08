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
        private WorkerActivityControl[] _gasControls;
        private WorkerActivityControl[] _menuControls;

        private const string TOWNHALL_CONTROL_NAME = "TownHallControl";
        private const string IDLE_CONTROL_NAME = "ConstructionControl";
        private const string MINERALS_CONTROL_NAME = "MineralsControl";
        private const string GAS_CONTROL_A_NAME = "GasControl_A";
        private const string GAS_CONTROL_B_NAME = "GasControl_B";

        //Const

        private const double WORKER_MINERALS_PER_SECOND = 1.0;
        private const double WORKER_GAS_PER_SECOND = 1.0;
        private const int INITIAL_WORKERS = 12;

        public override void _Ready()
        {
            _townhallControl = GetNode<ProductionBuildingControl>(TOWNHALL_CONTROL_NAME);
            _constructionControl = GetNode<WorkerActivityControl>(IDLE_CONTROL_NAME);
            _mineralsControl = GetNode<WorkerActivityControl>(MINERALS_CONTROL_NAME);
            _gasControls = [
                GetNode<WorkerActivityControl>(GAS_CONTROL_A_NAME), 
                GetNode<WorkerActivityControl>(GAS_CONTROL_B_NAME)];

            _menuControls = [_constructionControl, _mineralsControl, _gasControls[0], _gasControls[1]];

             //TODO implement same for townhall control

            _constructionControl.Init(WorkerActivityTypes.Construction);
            _mineralsControl.Init(WorkerActivityTypes.Minerals);

            foreach (var gasControl in _gasControls)
                gasControl.Init(WorkerActivityTypes.Gas);

            //Callbacks
            _townhallControl.UnitProduced += OnWorkerProduced;

            foreach (var menuControl in _menuControls)
                menuControl.MenuOpened += OnControlMenuOpened;
        }

        public override void _Process(double delta)
        {
            //TODO move econ updates to signals emitted from workerActivityControls once we move to individual workers
            //Update Economy
            if (_mineralsControl.WorkerCount > 0)
                EmitSignal(SignalName.MineralsMined, _mineralsControl.WorkerCount * WORKER_MINERALS_PER_SECOND * delta);

            foreach (var gasControl in _gasControls)
                if (gasControl.WorkerCount > 0)
                    EmitSignal(SignalName.GasMined, gasControl.WorkerCount * WORKER_GAS_PER_SECOND * delta);
        }

        public void Init()
        {
            Assert.That(PaymentProcessor is not null, "Payment Processor Uninitialized");

            _mineralsControl.WorkerCount = INITIAL_WORKERS;

            _townhallControl.PaymentProcessor = PaymentProcessor;
            _townhallControl.Init();
        }

        private void OnWorkerProduced()
        {
            _mineralsControl.WorkerCount++;
        }

        private void OnControlMenuOpened(WorkerActivityControl control)
        {
            foreach (var menuControl in _menuControls)
            {
                if (menuControl == control)
                {
                    menuControl.MoveToFront();
                }
                else
                {
                    menuControl.CloseMenu();
                }
            }
        }
    }
}