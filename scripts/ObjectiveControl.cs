using Godot;
using System;

namespace starcraftbuildtrainer.scripts
{
    public partial class ObjectiveControl : Control
    {
        [Signal] public delegate void ObjectiveStateChangeEventHandler(ObjectiveState state);

        private Label _objectiveLabel;
        private Label _timerLabel;

        private ObjectiveState _state;

        private double _timerSeconds = 0;

        private const string OBJECTIVE_LABEL_NAME = "ObjectiveLabel";
        private const string TIMER_LABEL_NAME = "TimerLabel";

        private readonly TimeSpan INITIAL_TIMER_SPAN = new(0, 1, 15);
        private const string TIMER_FORMAT = @"mm\:ss";

        private const int WORKER_WIN_COUNT = 16;
        private const string OBJECTIVE_FORMAT = "Build {0} SCVs";

        public override void _Ready()
        {
            _objectiveLabel = GetNode<Label>(OBJECTIVE_LABEL_NAME);
            _timerLabel = GetNode<Label>(TIMER_LABEL_NAME);

            Init();
        }

        public override void _Process(double delta)
        {
            if (_state != ObjectiveState.InProgress)
                return;

            //Update Timer
            _timerSeconds -= delta;
            _timerLabel.Text = TimeSpan.FromSeconds(_timerSeconds).ToString(TIMER_FORMAT);

            if (_timerSeconds <= 0)
                EmitSignal(SignalName.ObjectiveStateChange, (int)ObjectiveState.Failed);
        }

        public void Init()
        {
            _state = ObjectiveState.InProgress;
            _objectiveLabel.Text = string.Format(OBJECTIVE_FORMAT, WORKER_WIN_COUNT);
            _timerSeconds = INITIAL_TIMER_SPAN.TotalSeconds;
        }

        public void CheckObjectiveComplete(double workerCount)
        {
            if (workerCount >= WORKER_WIN_COUNT)
                EmitSignal(SignalName.ObjectiveStateChange, (int)ObjectiveState.Complete);
        }
    }
}