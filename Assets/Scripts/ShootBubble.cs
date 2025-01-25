using UnityEngine;

public class ShootBubble : MonoBehaviour
{

    private Player1InputActions inputActions;

    [SerializeField]
    private GameObject _bullet;



    private void Start()
    {
        inputActions = new Player1InputActions();
        inputActions.Player.Enable();

    }

    private void OnShoot() {
        Debug.Log("OnShoot");
        Instantiate(_bullet, transform.position, transform.rotation, null);
    }

}
