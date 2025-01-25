using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance { get; private set; }

    private bool gameOver = false;
    private Player winningPlayer;

    [SerializeField]
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


    public void OnPlayerLoose(Player player)
    {
        if (player == Player.Player1)
        {
            this.winningPlayer = Player.Player2;
            SoundController.Instance.PlaySound(SfxIdentifier.Player2Wins);
        }
        else
        {
            this.winningPlayer = Player.Player1;
            SoundController.Instance.PlaySound(SfxIdentifier.Player1Wins);
        }

        gameOver = true;
        Time.timeScale = 0;

        Debug.Log("Player Won: " + winningPlayer.ToString());
        gameOverScreen.OnGameOver(winningPlayer);
    }

}
