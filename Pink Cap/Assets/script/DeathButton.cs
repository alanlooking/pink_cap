using UnityEngine;
using UnityEngine.UI;

public class DeathButton : MonoBehaviour
{
    [Header("UI Подсказка")]
    [SerializeField] private GameObject hintCanvas;
    [SerializeField] private Text hintText;

    [Header("Звуковые эффекты")]
    [SerializeField] private AudioSource buttonAudioSource;
    [SerializeField] private AudioClip finalClickClip;

    private bool isActive = false;
    private bool playerInside = false;
    private int pressCount = 0;
    private PlayerDeath playerDeathScript;

    private const string FIRST_HINT = "Press E to destroy ALL robots";
    private const string CONFIRM_HINT = "Are you sure? Press E again to confirm";

    void Update()
    {
        if (!isActive || !playerInside) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            pressCount++;

            if (pressCount == 1)
            {
                if (hintText != null)
                {
                    hintText.text = CONFIRM_HINT;
                }
            }
            else if (pressCount >= 2)
            {
                if (buttonAudioSource != null && finalClickClip != null)
                {
                    buttonAudioSource.PlayOneShot(finalClickClip);
                }

                TriggerPlayerDeath();
            }
        }
    }

    public void ActivateButton()
    {
        isActive = true;

        if (playerInside)
        {
            ShowHint(FIRST_HINT);
        }
    }

    private void TriggerPlayerDeath()
    {
        if (hintCanvas != null) hintCanvas.SetActive(false);

        if (playerDeathScript != null)
        {
            playerDeathScript.Die(true);
        }
    }

    private void ShowHint(string message)
    {
        if (hintCanvas != null && hintText != null)
        {
            hintText.text = message;
            hintCanvas.SetActive(true);
        }
    }

    private void HideHint()
    {
        if (hintCanvas != null)
        {
            hintCanvas.SetActive(false);
        }
        pressCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            playerDeathScript = collision.GetComponent<PlayerDeath>();

            if (isActive)
            {
                ShowHint(FIRST_HINT);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            HideHint();
        }
    }
}