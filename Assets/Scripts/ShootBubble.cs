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

        Vector2 inputVector = inputActions.Player.Aim.ReadValue<Vector2>();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputVector);

        Vector2 shootDirection = mousePosition - (Vector2) transform.position;

        Debug.Log(shootDirection);

         var bullet = Instantiate(_bullet, transform.position, Quaternion.identity, null);
            bullet.transform.right = shootDirection;
    }

}
