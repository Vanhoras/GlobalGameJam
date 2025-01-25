using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBullet : MonoBehaviour
{
    [SerializeField]
    float speed = 20;
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
