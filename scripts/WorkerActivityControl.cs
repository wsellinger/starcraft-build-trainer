using Godot;

namespace starcraftbuildtrainer.scripts
{
    public partial class WorkerActivityControl : Control
    {
        public string Text { get => _workerCountLabel.Text; set => _workerCountLabel.Text = value; }

        private TextureRect _activityTextureRect;
        private Label _workerCountLabel;

        private const string ACTIVITY_TEXTURE_BOX_NAME = "ActivityTextureBox";
        private const string ACTIVITY_TEXTURE_NAME = "ActivityTexture";
        private const string WORKER_COUNT_LABEL_BOX_NAME = "WorkerCountLabelBox";
        private const string WORKER_COUNT_LABEL_NAME = "WorkerCountLabel";
        
        //TODO move worker counts into here
        //TODO implement construction buttons
        //TODO implement construction of supply depot
        //TODO implement supply
        //TODO implement construction of refinery
        //TODO implement gotoMinerals and gotoGas on the appropriate controls

        public override void _Ready()
        {
            _activityTextureRect = GetNode<BoxContainer>(ACTIVITY_TEXTURE_BOX_NAME).GetNode<TextureRect>(ACTIVITY_TEXTURE_NAME);
            _workerCountLabel = GetNode<BoxContainer>(WORKER_COUNT_LABEL_BOX_NAME).GetNode<Label>(WORKER_COUNT_LABEL_NAME);
        }

        public override void _Process(double delta)
        {
        }

        public void LoadActivityTexture(string path) => _activityTextureRect.Texture = GD.Load<Texture2D>(path);
    }
}