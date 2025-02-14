using Godot;

public partial class GameOverMenu : Control
{
    private Button _restartButton;
    private Button _quitButton;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.WhenPaused;
        _restartButton = GetNode<Button>("%RestartButton");
        _restartButton.Pressed += OnRestartButtonPressed;
        _quitButton = GetNode<Button>("%QuitButton");
        _quitButton.Pressed += OnQuitButtonPressed;
    }

    public void GameOver()
    {
        GetTree().Paused = true;
        Visible = true;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    private void OnRestartButtonPressed()
    {
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
