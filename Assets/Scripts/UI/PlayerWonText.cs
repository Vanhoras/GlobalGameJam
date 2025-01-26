using TMPro;
using UnityEngine;

public class PlayerWonText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text tmp;
   

    public void ShowPlayerWon(Player player)
    {
        if (player == Player.Player1)
        {
            tmp.text = "Player 1 Wins!";
        }
        else if (player == Player.Player2)
        {
            tmp.text = "Player 2 Wins!";
        }
    }
}
