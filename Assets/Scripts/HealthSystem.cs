using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float flashDuration = 0.2f;
    private SpriteRenderer spriteRenderer;
    private PauseMenu pauseMenu;
    private Player playerRef;
    private Color originalColor;
    private Coroutine flashCoroutine;
    public bool isInvulnerable = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Start()
    {
        playerRef = gameObject.GetComponent<Player>();
    }

    public void ReceiveDamage(float damage)
    {
        if (isInvulnerable) return;

        health -= damage;

        if (spriteRenderer != null)
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashRed());
        }

        if (health <= 0)
        {
            if (playerRef != null)
            {
                pauseMenu = GameObject.FindGameObjectWithTag("UI").GetComponent<PauseMenu>();
                pauseMenu.GameOver();
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
}
