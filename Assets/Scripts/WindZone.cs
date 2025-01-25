using UnityEngine;

public class WindZone : MonoBehaviour
{
    [SerializeField]
    private float strength = 0.1f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            return;
        }

        var rBody = collision.attachedRigidbody;

        if (rBody == null)
        {
            return;
        }

        Debug.Log("foo");
        var direction = transform.TransformDirection(Vector2.up);
        rBody.AddRelativeForce(direction * strength);
    }
}
