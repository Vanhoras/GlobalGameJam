using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    [SerializeField]
    private Health _health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            return;
        }

        var bullet = collision.GetComponent<Bullet>();

        _health.OnHeadWasHit(bullet);
    }
}
