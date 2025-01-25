using UnityEngine;

public class Bullet : MonoBehaviour
{
    private enum CollisionTarget
    {
        Level,
        PlayerHead,
        Nothing
    }

    [SerializeField]
    float speed = 20;

    public Player Origin { get; set; } = Player.Player1;

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        handleHeadCollision(collision, out var collisionTarget);

        if(collisionTarget == CollisionTarget.Level)
        {
            SoundController.Instance.PlaySound(SfxIdentifier.BubbleWallPop);
        }

        if (collisionTarget == CollisionTarget.PlayerHead)
        {
            SoundController.Instance.PlaySound(SfxIdentifier.BubblePlayerHit);
        }

        if (collisionTarget != CollisionTarget.Nothing)
        {
            Destroy(gameObject);
        }
    }

    private void handleHeadCollision(Collider2D collision, out CollisionTarget collisionTarget)
    {
        if (collision.gameObject.tag != "Player")
        {
            collisionTarget = CollisionTarget.Level;
            return;
        }

        var head = collision.gameObject.GetComponent<HeadCollider>();

        if (head == null)
        {
            collisionTarget = CollisionTarget.Nothing;
            return;
        }

        if (head.Player == Origin)
        {
            collisionTarget = CollisionTarget.Nothing;
            return;
        }

        collisionTarget = CollisionTarget.PlayerHead;
        head.OnBulletHit(this);
    }
}
