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

        movement.Knockback(bullet.knockbackForce, bullet.Direction);

        _health.OnBulletHit();
    }
}
