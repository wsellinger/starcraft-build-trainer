using Godot;
using System;

namespace starcraftbuildtrainer.scripts
{
    public partial class ResourceNodeControl : Control
    {
        public string Text { get => _workersLabel.Text; set => _workersLabel.Text = value; }

        private const string WORKERS_LABEL_BOX_NAME = "WorkerLabelBox";
        private const string WORKERS_LABEL_NAME = "WorkersLabel";
        private Label _workersLabel;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _workersLabel = GetNode<BoxContainer>(WORKERS_LABEL_BOX_NAME).GetNode<Label>(WORKERS_LABEL_NAME);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
    }
}