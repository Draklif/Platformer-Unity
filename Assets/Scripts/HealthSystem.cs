using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float health;

    public void ReceiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
