using UnityEngine;
using UnityEngine.InputSystem;

public class ShootBubble : MonoBehaviour
{
    private Player1InputActions inputActions1;
    private Player2InputActions inputActions2;

    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private PlayerMetadata _player;

    private void Start()
    {
        inputActions1 = new Player1InputActions();
        inputActions2 = new Player2InputActions();

        if (_player.Player == Player.Player1)
        {
            inputActions1.Player.Enable();
            inputActions1.Player.Shoot.performed += OnShoot;
        } else
        {
            inputActions2.Player.Enable();
            inputActions2.Player.Shoot.performed += OnShoot;
        }
        
    }

    private void OnDestroy()
    {
        if (_player.Player == Player.Player1)
        {
            inputActions1.Player.Shoot.performed -= OnShoot;
        }
        else
        {
            inputActions2.Player.Shoot.performed -= OnShoot;
        }
        
    }

    private void OnShoot(InputAction.CallbackContext input)
    {
        Vector2 inputVector;

        if (_player.Player == Player.Player1)
        {
            inputVector = inputActions1.Player.Aim.ReadValue<Vector2>();
        }
        else
        {
            Vector2 mousePosition = inputActions2.Player.Aim.ReadValue<Vector2>();
            inputVector = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        
        Vector2 shootDirection = inputVector - (Vector2)transform.position;

        var instance = Instantiate(_bullet, transform.position, Quaternion.identity, null);
        var defaultScale = instance.transform.localScale;
        var bullet = instance.GetComponent<Bullet>();
        
        bullet.transform.right = shootDirection;
        bullet.transform.localScale = defaultScale + defaultScale * 0.5f * (_player.Health.MaxHealth / Mathf.Max(1,((float)_player.Health.CurrentHealth)));
        bullet.Origin = _player.Player;
    }

}
