using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ShootBubble : MonoBehaviour
{
    private Player2InputActions inputActions;

    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private PlayerMetadata _player;

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

    private void OnShoot(InputAction.CallbackContext input)
    {
        Vector2 inputVector = inputActions.Player.Aim.ReadValue<Vector2>();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputVector);
        Vector2 shootDirection = mousePosition - (Vector2)transform.position;

        var instance = Instantiate(_bullet, transform.position, Quaternion.identity, null);
        var defaultScale = instance.transform.localScale;
        var bullet = instance.GetComponent<Bullet>();
        
        bullet.transform.right = shootDirection;
        bullet.transform.localScale = defaultScale + defaultScale * 0.5f * (_player.Health.MaxHealth / Mathf.Max(1,((float)_player.Health.CurrentHealth)));
        bullet.Origin = _player.Player;
    }

}
