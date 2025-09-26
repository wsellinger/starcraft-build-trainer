using Godot;
using System;

namespace starcraftbuildtrainer.scripts
{

    public partial class ResourceControl : Control, IResourceManager
    {
        //Events

        public event Action<string> MessageDispatched;
        
        //Properties

        public double Minerals => _resources.Minerals;
        public double Gas => _resources.Gas;
        public Supply Supply => _resources.Supply;

        //Nodes

        private Label _mineralsLabel;
        private Label _gasLabel;
        private Label _supplyLabel;

        private const string MINERALS_LABEL_NAME = "MineralsLabel";
        private const string GAS_LABEL_NAME = "GasLabel";
        private const string SUPPLY_LABEL_NAME = "SupplyLabel";

        //Data

        private Resources _resources;

        //Defaults

        private const int INITIAL_MINERALS = 50;
        private const string PAYMENT_FAILED_MESSAGE_MINERALS = "Not Enough Minerals";
        private const string PAYMENT_FAILED_MESSAGE_GAS = "Not Enough Gas";
        private const string PAYMENT_FAILED_MESSAGE_SUPPLY = "Not Enough Supply";

        public ResourceControl()
        {
            _resources = new Resources(INITIAL_MINERALS, 0, new(0, 0));
        }

        public override void _Ready()
        {
            _mineralsLabel = GetNode<Label>(MINERALS_LABEL_NAME);
            _gasLabel = GetNode<Label>(GAS_LABEL_NAME);
            _supplyLabel = GetNode<Label>(SUPPLY_LABEL_NAME);
        }

        public override void _Process(double delta)
        {
            _mineralsLabel.Text = Mathf.Round(_resources.Minerals).ToString();
            _gasLabel.Text = Mathf.Round(_resources.Gas).ToString();
            _supplyLabel.Text = $"{Supply.Used} / {Supply.Total}";
        }

        public void Init(Supply supply)
        {
            _resources.Supply = supply;
        }

        public void AddMinerals(double value) => _resources.Minerals += value;
        public void AddGas(double value) => _resources.Gas += value;

        public void AddSupplyTotal(uint value) => _resources.Supply.Total += value;

        public bool MakePayment(ResourceCost cost)
        {
            bool result = true;

            if (cost.Minerals > _resources.Minerals)
            {
                MessageDispatched.Invoke(PAYMENT_FAILED_MESSAGE_MINERALS);
                result = false;
            }

            if (cost.Gas > _resources.Gas)
            {
                MessageDispatched.Invoke(PAYMENT_FAILED_MESSAGE_GAS);
                result = false;
            }

            if (cost.Supply > _resources.Supply.Availiable)
            {
                MessageDispatched.Invoke(PAYMENT_FAILED_MESSAGE_SUPPLY);
                result = false;
            }
             
            if (result is true)
            {
                _resources -= cost;
            }

            return result;
        }

        private class Resources(uint minerals, uint gas, Supply supply)
        {
            public double Minerals { get; set; } = minerals;
            public double Gas { get; set; } = gas;
            public Supply Supply { get; set; } = supply;

            public static Resources operator -(Resources resource, ResourceCost cost)
            {
                resource.Minerals -= cost.Minerals;
                resource.Gas -= cost.Gas;
                resource.Supply.Used += cost.Supply;
                return resource;
            }
        }
    }

    public class Supply(uint total, uint used)
    {
        public uint Total { get; set; } = total;
        public uint Used { get; set; } = used;
        public uint Availiable => Total > Used ? Total - Used : 0;
    }
}

