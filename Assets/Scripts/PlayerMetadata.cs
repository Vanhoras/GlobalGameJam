using UnityEngine;

public class PlayerMetadata : MonoBehaviour
{
    [SerializeField]
    private Player player;

    public Player Player => player;

    public Health Health { get; private set; }

    private void Start()
    {
        Health = GetComponent<Health>();
    }
}
