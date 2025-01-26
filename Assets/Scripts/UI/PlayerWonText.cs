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
            tmp.text = "PLAYER 1";
        }
        else if (player == Player.Player2)
        {
            tmp.text = "PLAYER 2";
        }
    }
}
