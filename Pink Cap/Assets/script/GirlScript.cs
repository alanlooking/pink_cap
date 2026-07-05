using UnityEngine;

public class GirlDisappear : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float disappearDistance = 2f;
    [SerializeField] private string disappearTriggerName = "Disappear";

    [Header("Ссылка на кнопку смерти")]
    [SerializeField] private DeathButton targetButton;

    private Transform playerTransform;
    private Animator animator;
    private bool isDisappearing = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        movement player = FindFirstObjectByType<movement>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (isDisappearing) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartDisappearing();
            return;
        }

        if (playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= disappearDistance)
        {
            StartDisappearing();
        }
    }

    private void StartDisappearing()
    {
        isDisappearing = true;

        if (animator != null)
        {
            animator.SetTrigger(disappearTriggerName);
        }

        
        if (targetButton != null)
        {
            targetButton.ActivateButton();
        }

        
        Invoke(nameof(DisableCharacter), 1.5f);
    }

    private void DisableCharacter()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, disappearDistance);
    }
}