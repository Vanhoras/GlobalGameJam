using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBubble : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(_bullet, transform.position, transform.rotation, null);
        }
    }
}
