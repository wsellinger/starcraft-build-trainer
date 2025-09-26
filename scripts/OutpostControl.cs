using Godot;
using System;

namespace starcraftbuildtrainer.scripts
{
    public partial class OutpostControl : Control
    {
        //Events

        public event Action<double> MineralsMined;
        public event Action<double> GasMined;
        public event Action<string> MessageDispatched;

        //Properties

        public IResourceManager ResourceManager { private get; set; }
        public static Supply InitialSupply => new(TOWNHALL_SUPPLY, INITIAL_WORKERS);
        public int WorkerCount => _mineralsControl.WorkerCount;

        //Nodes

        private ProductionBuildingControl _townhallControl;
        private WorkerActivityControl _constructionControl;
        private WorkerActivityControl _mineralsControl;
        private WorkerActivityControl[] _gasControls;
        private WorkerActivityControl[] _menuControls;
        private GridContainer _buildingGrid;

        private const string TOWNHALL_CONTROL_NAME = "TownHallControl";
        private const string IDLE_CONTROL_NAME = "ConstructionControl";
        private const string MINERALS_CONTROL_NAME = "MineralsControl";
        private const string GAS_CONTROL_A_NAME = "GasControl_A";
        private const string GAS_CONTROL_B_NAME = "GasControl_B";
        private const string BUILDING_GRID_NAME = "BuildingGrid";

        //Data
        private Control[] _buildings;
        private float _columnWidth;

        //Const

        private const double WORKER_MINERALS_PER_SECOND = 1.0;
        private const double WORKER_GAS_PER_SECOND = 1.0;
        private const int INITIAL_WORKERS = 12;
        private const uint TOWNHALL_SUPPLY = 15; //TODO move to building data

        public override void _Ready()
        {
            _townhallControl = GetNode<ProductionBuildingControl>(TOWNHALL_CONTROL_NAME);
            _constructionControl = GetNode<WorkerActivityControl>(IDLE_CONTROL_NAME);
            _mineralsControl = GetNode<WorkerActivityControl>(MINERALS_CONTROL_NAME);
            _gasControls = [
                GetNode<WorkerActivityControl>(GAS_CONTROL_A_NAME), 
                GetNode<WorkerActivityControl>(GAS_CONTROL_B_NAME)];
            _buildingGrid = GetNode<GridContainer>(BUILDING_GRID_NAME);

            _menuControls = [_constructionControl, _mineralsControl, _gasControls[0], _gasControls[1]];

             //TODO implement same for townhall control

            _constructionControl.Init(WorkerActivityControlData.Construction);
            _mineralsControl.Init(WorkerActivityControlData.Minerals);

            foreach (var gasControl in _gasControls)
                gasControl.Init(WorkerActivityControlData.Gas);

            _columnWidth = _buildingGrid.Size.X / _buildingGrid.Columns;
            
            //Callbacks
            _townhallControl.UnitProduced += OnWorkerProduced;
            _townhallControl.MessageDispatched += OnMessageDispatched;

            foreach (var menuControl in _menuControls)
            {
                menuControl.MenuOpened += OnControlMenuOpened;
                menuControl.ActionSelected += OnActionSelected;
            }
        }

        public override void _Process(double delta)
        {
            //TODO move econ updates to signals emitted from workerActivityControls once we move to individual workers
            //Update Economy
            if (_mineralsControl.WorkerCount > 0)
                MineralsMined.Invoke(_mineralsControl.WorkerCount * WORKER_MINERALS_PER_SECOND * delta);

            foreach (var gasControl in _gasControls)
                if (gasControl.WorkerCount > 0)
                    GasMined.Invoke(gasControl.WorkerCount * WORKER_GAS_PER_SECOND * delta);
        }

        public void Init()
        {
            Assert.That(ResourceManager is not null, "Payment Processor Uninitialized");

            _mineralsControl.WorkerCount = INITIAL_WORKERS;

            _townhallControl.PaymentProcessor = ResourceManager;
            _townhallControl.Init();
        }

        private void OnWorkerProduced()
        {
            _mineralsControl.WorkerCount++;
        }

        private void OnControlMenuOpened(WorkerActivityControl eventDispatcher)
        {
            foreach (var menuControl in _menuControls)
            {
                if (menuControl == eventDispatcher)
                {
                    menuControl.MoveToFront();
                }
                else
                {
                    menuControl.CloseMenu();
                }
            }
        }

        private void OnActionSelected(WorkerActivityControl eventDispatcher, ActionButtonData data)
        {
            switch (data)
            {
                case GatherButtonData gatherButtonData:
                    //TODO move worker from one control to another
                    break;
                case BuildButtonData buildButtonData:
                    BuildBuilding(buildButtonData.Type);
                    break;
                default:
                    break;
            }
        }

        private void OnMessageDispatched(string message)
        {
            MessageDispatched.Invoke(message);
        }

        private void BuildBuilding(BuildingIdentity identity)
        {
            var data = BuildingData.Map[identity];
            if (ResourceManager.MakePayment(data.Cost))
            {
                var building = new BuildingControl(data, _columnWidth);
                building.ConstructionComplete += OnBuildingConstructionComplete;
                _buildingGrid.AddChild(building);
            }
        }

        private void OnBuildingConstructionComplete(BuildingData data)
        {
            if (data.Supply > 0)
            {
                ResourceManager.AddSupplyTotal(data.Supply);
            }            
        }
    }
}