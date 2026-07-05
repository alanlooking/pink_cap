using UnityEngine;

public class GirlDisappear : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float disappearDistance = 2f;
    [SerializeField] private string disappearTriggerName = "Disappear";
    [SerializeField] private float destroyDelay = 1f;

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
        if (isDisappearing || playerTransform == null) return;

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

        Destroy(gameObject, destroyDelay);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, disappearDistance);
    }
}