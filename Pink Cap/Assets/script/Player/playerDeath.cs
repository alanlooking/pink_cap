using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI; 

public class PlayerDeath : MonoBehaviour
{
    public float restartDelay = 1f;

    [Header("Компоненты финальных титров (только для финала)")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private Text finalLine1; 
    [SerializeField] private Text finalLine2; 
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Эффект обычной смерти")]
    [SerializeField] private RectTransform deathCircleRect;
    [SerializeField] private float circleExpandSpeed = 20f; 

    private bool isDead = false;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private movement movementScript;
    private Animator animator;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        movementScript = GetComponent<movement>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    public void Die(bool isFinalDeath = false)
    {
        if (isDead)
            return;

        isDead = true;

        if (animator != null)
        {
            animator.SetBool("isDeath", true);
        }

        if (movementScript != null) movementScript.enabled = false;
        if (playerCollider != null) playerCollider.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
        }

        if (isFinalDeath)
        {
            StartCoroutine(FinalCutsceneSequence());
        }
        else
        {
            
            StartCoroutine(DeathCircleSequence());
        }
    }

    private IEnumerator DeathCircleSequence()
    {
        
        
        yield return new WaitForSeconds(0.65f);

        
        if (deathCircleRect != null && mainCamera != null)
        {
            
            Vector2 screenPosition = mainCamera.WorldToScreenPoint(transform.position);

            
            deathCircleRect.position = screenPosition;
            deathCircleRect.localScale = Vector3.zero;
            deathCircleRect.gameObject.SetActive(true);

            
            float currentScale = 0f;
            
            while (currentScale < 35f)
            {
                currentScale += circleExpandSpeed * Time.deltaTime;
                deathCircleRect.localScale = new Vector3(currentScale, currentScale, 1f);
                yield return null;
            }
        }

        
        yield return new WaitForSeconds(0.2f);

        
        RestartLevel();
    }

    private IEnumerator FinalCutsceneSequence()
    {
        float time = 0;
        float fadeDuration = 1.5f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            }
            yield return null;
        }
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 1f;

        yield return new WaitForSeconds(0.5f);

        if (finalLine1 != null)
        {
            finalLine1.text = "You were my favorite android...";
            finalLine1.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2.0f);

        if (finalLine2 != null)
        {
            finalLine2.text = "...until you get out of hand.";
            finalLine2.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(4.0f);

        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}