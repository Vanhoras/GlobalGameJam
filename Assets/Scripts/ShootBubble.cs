using UnityEngine;
using UnityEngine.InputSystem;

public class ShootBubble : MonoBehaviour
{
    private Player2InputActions inputActions;

    [SerializeField]
    private GameObject _bullet;

    private void Start()
    {
        inputActions = new Player2InputActions();
        inputActions.Player.Enable();
        inputActions.Player.Shoot.performed += OnShoot;
    }

    private void OnDestroy()
    {
        inputActions.Player.Shoot.performed -= OnShoot;
    }

    private void OnShoot(InputAction.CallbackContext input) {

        Vector2 inputVector = inputActions.Player.Aim.ReadValue<Vector2>();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputVector);
        Vector2 shootDirection = mousePosition - (Vector2) transform.position;

        Debug.Log(shootDirection);

         var bullet = Instantiate(_bullet, transform.position, Quaternion.identity, null);
            bullet.transform.right = shootDirection;
    }

}
