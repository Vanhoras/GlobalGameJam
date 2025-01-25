using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int startHealth = 5;

    [SerializeField]
    private Transform headTransform;

    [SerializeField]
    private Rigidbody2D playerRigidbody;

    private float _defaultGravity;

    private void Start()
    {
        _defaultGravity = playerRigidbody.gravityScale;
    }

    private int _bulletsTaken;

    public void OnHeadWasHit(Bullet bullet)
    {
        _bulletsTaken++;
        UpdateHeadSize();
        UpdateGravity();

        Destroy(bullet.gameObject);
    }

    private void UpdateHeadSize()
    {
        headTransform.localScale = Vector3.one * (1 + _bulletsTaken / 5f);
    }

    private void UpdateGravity()
    {
        var decrement = _defaultGravity / (startHealth - 1);
        playerRigidbody.gravityScale -= decrement;
    }
}
