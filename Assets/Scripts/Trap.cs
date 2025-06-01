using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float attackDamage;
    [SerializeField] private float tickDamage;
    private bool isStillIn = false;
    private GameObject playerRef;

    IEnumerator DOT()
    {
        yield return new WaitForSeconds(tickDamage);
        DoDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHitbox"))
        {
            isStillIn = true;
            playerRef = collision.gameObject;
            DoDamage();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHitbox"))
        {
            isStillIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHitbox"))
        {
            isStillIn = false;
        }
    }

    private void DoDamage()
    {
        if (isStillIn) 
        {
            HealthSystem healthComponent = playerRef.GetComponent<HealthSystem>();
            healthComponent.ReceiveDamage(attackDamage);
            StartCoroutine(DOT());
        }
    }
}

