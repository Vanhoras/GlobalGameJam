using UnityEngine;
using UnityEngine.InputSystem;

public class ShootBubble : MonoBehaviour
{

    PlayerInput playerInput;
    InputAction aimAction;

    [SerializeField]
    private GameObject[] _bulletPrefabs;

    [SerializeField]
    private PlayerMetadata _player;

    [SerializeField] 
    private PlayerAnimationManager animationManager;

    [SerializeField]
    private Transform crosshair;

    Vector2 lastShootDirection = Vector2.zero;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if(crosshair == null)
        {
            return;
        }

        crosshair.transform.right = GetShootDirection();
    }

    public void OnAim(InputAction.CallbackContext input)
    {
        Debug.Log("OnAim");

    }

    public void OnShoot(InputAction.CallbackContext input)
    {
        Debug.Log("OnShoot");

        var shootDirection = GetShootDirection();

        GameObject bulletPrefab = _bulletPrefabs[Random.Range(0, _bulletPrefabs.Length - 1)];
        
        var instance = Instantiate(bulletPrefab, transform.position, Quaternion.identity, null);
        var defaultScale = instance.transform.localScale;
        var bullet = instance.GetComponent<Bullet>();
        
        bullet.transform.right = shootDirection;

        float sizeHealthCoefficient = 1 + (_player.Health.MaxHealth - Mathf.Max(1, ((float)_player.Health.CurrentHealth))) / 2;
        bullet.transform.localScale = defaultScale + defaultScale * 0.5f * sizeHealthCoefficient;

        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();

        bullet.Origin = _player.Player;
        bullet.Direction = shootDirection.x >= 0 ? 1 : -1;

        SoundController.Instance.PlaySound(SfxIdentifier.Shoot);

        animationManager.TriggerShootAnimation();
    }

    private Vector2 GetShootDirection()
    {
        Vector2 shootDirection;

        if (_player.Player == Player.Player1)
        {
            Vector3 gampadVector = Vector3.zero;
            shootDirection = new Vector2(gampadVector.x, gampadVector.y);

            if (shootDirection.x == 0 && shootDirection.y == 0)
            {
                shootDirection = lastShootDirection;
            }
        }
        else
        {
            Vector2 mousePosition = Vector2.zero;
            Vector2 inputVector = Camera.main.ScreenToWorldPoint(mousePosition);
            shootDirection = inputVector - (Vector2)transform.position;
        }

        lastShootDirection = shootDirection;

        return shootDirection;
    }

}
