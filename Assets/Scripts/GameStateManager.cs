using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private UIInputActions inputActions;

    public static GameStateManager instance { get; private set; }

    private bool gameOver = false;
    private Player winningPlayer;

    private GameOverScreen gameOverScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        inputActions = new UIInputActions();
        inputActions.UI.Enable();

        inputActions.UI.Restart.performed += OnRestart;
    }

    private void OnDestroy()
    {
        inputActions.UI.Restart.performed -= OnRestart;
    }


    public void OnPlayerLoose(Player player)
    {
        if (player == Player.Player1)
        {
            winningPlayer = Player.Player2;
            SoundController.Instance.PlaySound(SfxIdentifier.Player2Wins);
        }
        else
        {
            winningPlayer = Player.Player1;
            SoundController.Instance.PlaySound(SfxIdentifier.Player1Wins);
        }

        gameOver = true;
        Time.timeScale = 0;

        gameOverScreen = (GameOverScreen)GameObject.FindObjectOfType(typeof(GameOverScreen));

        Debug.Log("Player Won: " + winningPlayer.ToString());
        gameOverScreen.OnGameOver(winningPlayer);
    }

    private void OnRestart(InputAction.CallbackContext input)
    {
        if (!gameOver) return;

        gameOver = false;
        Time.timeScale = 1;

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        nextScene = nextScene >= SceneManager.sceneCountInBuildSettings ? 0 : nextScene;

        SceneManager.LoadScene(nextScene);
    }

}
