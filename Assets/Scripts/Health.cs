using Pixelplacement;
using Pixelplacement.TweenSystem;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int startHealth = 5;
    private int _bulletsTaken;

    [SerializeField]
    private Transform headTransform;

    [SerializeField]
    private Rigidbody2D playerRigidbody;

    private float _defaultGravity;

    public int MaxHealth => startHealth;

    public int CurrentHealth => Mathf.Max(0, MaxHealth - _bulletsTaken);

    private void Start()
    {
        _defaultGravity = playerRigidbody.gravityScale;
    }

    public void OnBulletHit()
    {
        _bulletsTaken++;
        UpdateHeadSize();
        UpdateGravity();
    }

    private void UpdateHeadSize()
    {
        var newSize = Vector3.one * (1 + _bulletsTaken / 5f);
        Tween.LocalScale(headTransform, newSize, 0.1f, 0, Tween.EaseOutBack);
    }

    private void UpdateGravity()
    {
        playerRigidbody.gravityScale = _defaultGravity * (CurrentHealth / (float)MaxHealth);
        if (playerRigidbody.gravityScale <= 0)
        {
            playerRigidbody.gravityScale = -1;

            playerRigidbody.excludeLayers = ~(1 << 10);
        }
    }
}
