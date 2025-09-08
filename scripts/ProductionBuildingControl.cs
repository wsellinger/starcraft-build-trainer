using Godot;
using System;

namespace starcraftbuildtrainer.scripts
{
    public partial class ProductionBuildingControl : Control
    {
        //Signals
        [Signal] public delegate void UnitProducedEventHandler();

        //References

        public IPaymentProcessor PaymentProcessor { private get; set; }

        //Nodes

        private Button _selectionButton;
        private ProductionButton _productionButton;
        private Label _queueLabel;
        private ProgressBar _progressBar;

        private const string SELECTION_BUTTON_NAME = "SelectionButton";
        private const string PROGRESS_BAR_NAME = "ProgressBar";
        private const string QUEUE_LABEL_NAME = "QueueLabel";

        //Data

        private double _workerBuildProgress;
        private int _workersInBuildQueue;

        //Const

        private const int UNIT_BUILD_TIME = 12;
        private const int MAX_UNITS_IN_QUEUE = 5;
        private readonly ResourceCost _workerResourceCost = new(50, 0);

        private const float PRODUCTION_BUTTON_X_BUFFER = 10;
        private const string UNIT_TEXTURE_PATH = "res://assets/art/terran/units/scv.png";

        public override void _Ready()
        {
            //Nodes
            _queueLabel = GetNode<Label>(QUEUE_LABEL_NAME);
            _selectionButton = GetNode<Button>(SELECTION_BUTTON_NAME);
            _progressBar = GetNode<ProgressBar>(PROGRESS_BAR_NAME);

            _productionButton = CreateProductionButton(_selectionButton);

            //Callbacks
            _selectionButton.Pressed += OnSelectionButtonPressed;
            _productionButton.Pressed += OnProductionButtonPressed;

            ProductionButton CreateProductionButton(Control parent)
            {
                float x = parent.Position.X + parent.Size.X + PRODUCTION_BUTTON_X_BUFFER;
                float size = parent.Size.Y / 3;

                var button = ProductionButton.Instantiate();
                button.TexturePath = UNIT_TEXTURE_PATH;
                button.Position = new Vector2(x, parent.Position.Y);
                button.Size = new Vector2(size, size);
                button.Visible = false;
                AddChild(button);

                return button;
            }
        }

        public override void _Process(double delta)
        {
            //Update Production
            if (_workersInBuildQueue > 0)
            {
                _workerBuildProgress += delta;

                if (_workerBuildProgress >= UNIT_BUILD_TIME)
                {
                    _workersInBuildQueue--;
                    EmitSignal(SignalName.UnitProduced);
                    _workerBuildProgress = 0;

                    if (_workersInBuildQueue == 0)
                    {
                        _progressBar.Hide();
                        _queueLabel.Hide();
                    }
                }
            }

            //Update Display
            _queueLabel.Text = _workersInBuildQueue.ToString();
            _progressBar.Value = _workerBuildProgress / UNIT_BUILD_TIME;
        }

        public void Init()
        {
            Assert.That(PaymentProcessor is not null, "Payment Processor Uninitialized");

            _workerBuildProgress = 0;
            _workersInBuildQueue = 0;

            _progressBar.Hide();
            _queueLabel.Hide();
        }

        private void OnSelectionButtonPressed()
        {
            _productionButton.Visible = !_productionButton.Visible;
        }

        private void OnProductionButtonPressed()
        {
            if (_workersInBuildQueue >= MAX_UNITS_IN_QUEUE)
            {
                //TODO Display Error
                return;
            }

            if (PaymentProcessor.MakePayment(_workerResourceCost))
            {
                _workersInBuildQueue++;
                _progressBar.Show();
                _queueLabel.Show();
            }
        }
    }
}
