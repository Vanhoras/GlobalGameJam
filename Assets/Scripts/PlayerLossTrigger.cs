using UnityEngine;

public class PlayerLossTrigger : MonoBehaviour
{

    private PlayerMetadata _player;

    void Start()
    {
        _player = GetComponent<PlayerMetadata>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 10)
        {
            GameStateManager.instance.OnPlayerLoose(_player.Player);
        }
    }
}
