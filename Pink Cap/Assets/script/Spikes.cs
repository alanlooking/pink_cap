using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что в шипы попал именно игрок
        if (collision.CompareTag("Player"))
        {
            PlayerDeath playerDeath = collision.GetComponent<PlayerDeath>();

            if (playerDeath != null)
            {
                playerDeath.Die();
            }
        }
    }
}