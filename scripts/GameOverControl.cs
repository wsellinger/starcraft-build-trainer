using Godot;

namespace starcraftbuildtrainer.scripts
{
    public partial class GameOverControl : Control
    {
        [Signal] public delegate void RestartEventHandler();

        public bool IsGameOver { get; set; }

        //Nodes

        private Control _victoryScreen;
        private Control _defeatScreen;
        private Button _restartButton;

        private const string VICTORY_SCREEN_NAME = "VictoryScreen";
        private const string DEFEAT_SCREEN_NAME = "DefeatScreen";
        private const string RESTART_BUTTON_NAME = "RestartButton";

        public override void _Ready()
        {
            _victoryScreen = GetNode<Control>(VICTORY_SCREEN_NAME);
            _defeatScreen = GetNode<Control>(DEFEAT_SCREEN_NAME);
            _restartButton = GetNode<Button>(RESTART_BUTTON_NAME);

            _restartButton.Pressed += OnRestartButtonPressed;
        }

        public void Init()
        {
            IsGameOver = false;

            _victoryScreen.Hide();
            _defeatScreen.Hide();
            _restartButton.Hide();
        }

        public void GameOver(ObjectiveState state)
        {
            switch (state)
            {
                case ObjectiveState.Complete:
                    GameOver(_victoryScreen);
                    break;
                case ObjectiveState.Failed:
                    GameOver(_defeatScreen);
                    break;
                default:
                    throw new System.Exception("Unexpected Objective State");
            }
        }

        private void GameOver(Control screen)
        {
            screen.Show();
            _restartButton.Show();
            IsGameOver = true;
        }

        private void OnRestartButtonPressed()
        {
            EmitSignal(SignalName.Restart);
        }
    }
}
