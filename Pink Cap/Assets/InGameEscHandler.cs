using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InGameEscHandler : MonoBehaviour
{
    [Header("Настройки перехода")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isExiting = false;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape) && !isExiting)
        {
            StartCoroutine(ExitToMenuSequence());
        }
    }

    private IEnumerator ExitToMenuSequence()
    {
        isExiting = true;

        
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

        
        yield return new WaitForSeconds(0.1f);

        
        SceneManager.LoadScene(mainMenuSceneName);
    }
}