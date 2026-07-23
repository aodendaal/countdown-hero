using UnityEngine;

public class OverworldEnemy : MonoBehaviour
{
    [SerializeField] private MonsterData monsterData;

    private void OnTriggerEnter2D(Collider2D other) // Use OnTriggerEnter for 3D
    {
        if (other.CompareTag("Player"))
        {
            // Optional: Destroy or disable overworld sprite so it disappears while fighting
            // gameObject.SetActive(false); 

            // Trigger combat state pass data
            GameStateManager.Instance.StartCombatWith(monsterData, this.gameObject);
        }
    }
}