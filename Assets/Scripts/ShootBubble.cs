using UnityEngine;
using UnityEngine.InputSystem;

public class ShootBubble : MonoBehaviour
{

    PlayerInput playerInput;
    InputAction aimAction;

    [SerializeField]
    private GameObject _bullet;



    private void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();

        aimAction = playerInput.actions["Aim"];
    }

    public void OnShoot(InputAction.CallbackContext context) {

        Debug.Log("OnShoot");

        Vector2 inputVector = aimAction.ReadValue<Vector2>();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputVector);

        Vector2 shootDirection = mousePosition - (Vector2) transform.position;

        Debug.Log(shootDirection);

         var bullet = Instantiate(_bullet, transform.position, Quaternion.identity, null);
            bullet.transform.right = shootDirection;
    }

}
