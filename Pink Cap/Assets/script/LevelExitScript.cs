using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelExitZone : MonoBehaviour
{
    [Header("Настройки перехода")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning && !string.IsNullOrEmpty(nextSceneName))
        {
            StartCoroutine(TransitionToNextLevel());
        }
    }

    private IEnumerator TransitionToNextLevel()
    {
        isTransitioning = true;

        movement playerMovement = FindFirstObjectByType<movement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        Rigidbody2D playerRb = playerMovement?.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.bodyType = RigidbodyType2D.Kinematic;
        }

        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            }
            yield return null;
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
        }

        if (nextSceneName.StartsWith("Level "))
        {
            string levelNumberString = nextSceneName.Replace("Level ", "");
            if (int.TryParse(levelNumberString, out int nextLevelNumber))
            {
                int currentSavedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

                if (nextLevelNumber > currentSavedLevel)
                {
                    PlayerPrefs.SetInt("ReachedLevel", nextLevelNumber);
                    PlayerPrefs.Save();
                }
            }
        }

        SceneManager.LoadScene(nextSceneName);
    }
}