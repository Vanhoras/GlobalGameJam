using UnityEngine;
using UnityEngine.UIElements;

public class ShootBubble : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;
    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var shootDirection = mousePosition - transform.position;
        shootDirection = new Vector2(shootDirection.x, shootDirection.y);

        Debug.Log(shootDirection);
        
        if (Input.GetButtonDown("Fire1"))
        {
            var bullet = Instantiate(_bullet, transform.position, Quaternion.identity, null);
            bullet.transform.right = shootDirection;
        }
    }
}
