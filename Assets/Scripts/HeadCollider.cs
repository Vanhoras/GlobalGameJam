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

    public void OnBulletHit(Bullet bullet)
    {
        // Player should not be able to hit theirselves
        if (bullet.Origin == _player.Player)
        {
            return;
        }

        int bulletDirection = bullet.transform.position.x >= transform.position.x ? -1 : 1;

        movement.Knockback(bullet.knockbackForce, bulletDirection);

        _health.OnBulletHit();
    }
}
