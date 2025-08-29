using Godot;

namespace starcraftbuildtrainer.scripts
{

    public partial class ResourceControl : Control, IPaymentProcessor
    {
        public double Minerals { get => _resources.Minerals; }
        public double Gas { get => _resources.Gas; }

        //Nodes

        private Label _mineralsLabel;
        private Label _gasLabel;

        private const string MINERALS_LABEL_NAME = "MineralsLabel";
        private const string GAS_LABEL_NAME = "GasLabel";

        //Data

        private Resources _resources;

        //Defaults

        private const int INITIAL_MINERALS = 50;
        private const int INITIAL_GAS = 0;

        public override void _Ready()
        {
            _mineralsLabel = GetNode<Label>(MINERALS_LABEL_NAME);
            _gasLabel = GetNode<Label>(GAS_LABEL_NAME);
        }

        public override void _Process(double delta)
        {
            _mineralsLabel.Text = Mathf.Round(_resources.Minerals).ToString();
            _gasLabel.Text = Mathf.Round(_resources.Gas).ToString();
        }

        public void Init()
        {
            _resources.Minerals = INITIAL_MINERALS;
            _resources.Gas = INITIAL_GAS;
        }

        public void AddMinerals(double value) => _resources.Minerals += value;
        public void AddGas(double value) => _resources.Gas += value;

        public bool MakePayment(ResourceCost cost)
        {
            bool result = true;

            if (cost.Minerals > _resources.Minerals)
            {
                //TODO display error
                result = false;
            }

            if (cost.Gas > _resources.Gas)
            {
                //TODO display error
                result = false;
            }

            //TODO add supply check

            if (result is true)
            {
                _resources -= cost;
            }

            return result;
        }

        private struct Resources(uint minerals, uint gas)
        {
            public double Minerals { get; set; } = minerals;
            public double Gas { get; set; } = gas;

            public static implicit operator Resources(ResourceCost cost) => new(cost.Minerals, cost.Gas);

            public static Resources operator -(Resources minuend, Resources subtrahend)
            {
                minuend.Minerals -= subtrahend.Minerals;
                minuend.Gas -= subtrahend.Gas;
                return minuend;
            }
        }
    }
}

