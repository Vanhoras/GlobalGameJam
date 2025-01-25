using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float speed = 20;

    public Player Origin { get; set; } = Player.Player1;

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        handleHeadCollision(collision, out var shouldDestroy);

        if (shouldDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void handleHeadCollision(Collider2D collision, out bool shouldDestroy)
    {
        if (collision.gameObject.tag != "Player")
        {
            shouldDestroy = true;
            return;
        }

        var head = collision.gameObject.GetComponent<HeadCollider>();

        if (head == null)
        {
            shouldDestroy = false;
            return;
        }

        if (head.Player == Origin)
        {
            shouldDestroy = false;
            return;
        }

        shouldDestroy = true;
        head.OnBulletHit(this);
    }
}
