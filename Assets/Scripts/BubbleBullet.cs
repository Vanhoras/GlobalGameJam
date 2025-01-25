using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBullet : MonoBehaviour
{
    [SerializeField]
    float speed = 20;
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }
}
