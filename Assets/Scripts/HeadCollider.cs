using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    [SerializeField]
    private Health _health;

    [SerializeField]
    private PlayerMetadata _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            return;
        }

        var bullet = collision.GetComponent<Bullet>();

        // Player should not be able to hit theirselves
        if(bullet.Origin == _player.Player)
        {
            return;
        }

        _health.OnHeadWasHit(bullet);
    }
}
