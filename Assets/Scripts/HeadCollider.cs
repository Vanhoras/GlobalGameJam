using SmoothShakeFree;
using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    [SerializeField]
    private Health _health;

    [SerializeField]
    private PlayerMetadata _player;

    [SerializeField]
    private Movement movement;

    public Player Player => _player.Player;

    private Collider2D collider;

    private void Start()
    {
        collider = GetComponent<Collider2D>();

        collider.excludeLayers = 1 << 6;
    }

    public void OnBulletHit(Bullet bullet)
    {
        // Player should not be able to hit theirselves
        if (bullet.Origin == _player.Player)
        {
            return;
        }

        movement.Knockback(bullet.knockbackForce, bullet.Direction);
        Camera.main.GetComponent<ShakeBase>().StartShake();

        _health.OnBulletHit();
    }
}
