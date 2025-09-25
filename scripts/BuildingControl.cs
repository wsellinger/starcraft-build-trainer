using Godot;
using System;

namespace starcraftbuildtrainer.scripts
{
    public partial class BuildingControl(BuildingData data, float columnWidth) : Control
    {
        //Events

        public event Action<BuildingIdentity> ConstructionComplete;

        //Properties

        public BuildingData Data { get; init; } = data;

        private const int PROGRESS_BAR_HEIGHT_RATIO = 5;

        //Children

        private TextureRect _display;
        private ProgressBar _progressBar;

        //Data

        private bool _constructed = false;
        private double _progress = 0;

        private readonly float _columnWidth = columnWidth;

        public override void _Ready()
        {
            _display = new()
            {
                Texture = ResourceLoader.Load<Texture2D>(Data.PendingTexturePath),
                ExpandMode = TextureRect.ExpandModeEnum.FitHeightProportional,
                CustomMinimumSize = new Vector2(_columnWidth, 0)
            };
            AddChild(_display);

            _progressBar = new()
            {
                Size = new(_display.Size.X, _display.Size.Y / PROGRESS_BAR_HEIGHT_RATIO),
                Position = new(0, _display.Size.Y),
                MinValue = 0,
                MaxValue = 1
            };
            AddChild(_progressBar);

            Size = new(_display.Size.X, _display.Size.Y + _progressBar.Size.Y);
            CustomMinimumSize = Size;
        }

        public override void _Process(double delta)
        {
            if (!_constructed)
            {
                _progress += delta;
                _progressBar.Value = Math.Clamp(_progress / Data.BuildTime, 0, 1);

                if (_progress >= Data.BuildTime)
                {
                    _progressBar.Hide();
                    _display.Texture = ResourceLoader.Load<Texture2D>(Data.CompleteTexturePath);
                    ConstructionComplete?.Invoke(Data.Identity);
                }
            }
        }
    }
}
