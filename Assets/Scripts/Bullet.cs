using System.Buffers;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float speed = 20;

    public Player Origin {get;set; } = Player.Player1;

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
