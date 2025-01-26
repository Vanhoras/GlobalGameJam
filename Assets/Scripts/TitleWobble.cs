using Pixelplacement;
using UnityEngine;

public class TitleWobble : MonoBehaviour
{
    [SerializeField]
    public Transform _text;

    // Start is called before the first frame update
    void Start()
    {
        Tween.Rotation(_text, Quaternion.Euler(0, 0, 3), Quaternion.Euler(0, 0, -3), 1f, 0, Tween.EaseInOut, Tween.LoopType.PingPong);
    }
}
