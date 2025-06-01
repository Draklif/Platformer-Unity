using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody2D rb;
    public float attackDamage;
    public bool isFriendly = false;

    [SerializeField] private float impulsoDisparo;

    public void Disparar(float direccion)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.right * direccion * impulsoDisparo, ForceMode2D.Impulse);
        StartCoroutine(TimeToLive());
    }

    IEnumerator TimeToLive()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHitbox") && !isFriendly)
        {
            HealthSystem healthComponent = collision.gameObject.GetComponent<HealthSystem>();
            healthComponent.ReceiveDamage(attackDamage);
            Destroy(gameObject);
        }

        if (!collision.gameObject.CompareTag("PlayerHitbox") && isFriendly)
        {
            HealthSystem healthComponent = collision.gameObject.GetComponent<HealthSystem>();
            if (healthComponent != null)
            {
                healthComponent.ReceiveDamage(attackDamage);
                Destroy(gameObject);
            }
        }
    }
}

