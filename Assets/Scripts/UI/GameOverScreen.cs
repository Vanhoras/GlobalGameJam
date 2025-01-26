using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    PlayerWonText playerWonText;

    [SerializeField]
    GameObject gameOverPanel;

    public void OnGameOver(Player player)
    {
        gameOverPanel.SetActive(true);

        playerWonText.ShowPlayerWon(player);
    }
}
