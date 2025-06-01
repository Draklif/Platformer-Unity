using System.Collections;
using UnityEngine;

public class PatrolEnemy: MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float velocidad;
    [SerializeField] private float waitTime;

    [Header("Combate")]
    [SerializeField] private bool hasAttack;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject attackObject;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackWaitTime;

    private Vector3 destinoActual;
    private int indice = 0;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();

        if (waypoints.Length > 0) destinoActual = waypoints[indice].position;
        StartCoroutine(Patrulla());
        if (hasAttack) { StartCoroutine(RutinaAtaque()); }
    }

    void Update()
    {
        
    }

    IEnumerator Patrulla()
    {
        while (waypoints.Length > 0) 
        {
            while (transform.position != destinoActual)
            {
                transform.position = Vector3.MoveTowards(transform.position, destinoActual, velocidad * Time.deltaTime);
                yield return null;
            }
            DefinirNuevoDestino();
            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator RutinaAtaque()
    {
        while (true)
        {
            anim.SetTrigger("atacar");
            yield return new WaitForSeconds(attackWaitTime);
        }
    }

    // Launched from anim event
    private void Atacar()
    {
        Instantiate(attackObject, attackPoint.position, transform.rotation);
    }

    private void DefinirNuevoDestino()
    {
        indice = indice >= waypoints.Length - 1 ? 0 : indice + 1;
        destinoActual = waypoints[indice].position;
        transform.localScale = destinoActual.x > transform.position.x ? Vector3.one : new Vector3(-1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDetection"))
        {
            Debug.Log("Detectado");
        }
        else if (collision.gameObject.CompareTag("PlayerHitbox")) 
        {
            HealthSystem healthComponent = collision.gameObject.GetComponent<HealthSystem>();
            healthComponent.ReceiveDamage(attackDamage);
        }
    }
}
