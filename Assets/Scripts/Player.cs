using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private GameObject feet;
    [SerializeField] private float velocidadMov;
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private float floorDetectionDistance;
    [SerializeField] private LayerMask whatIsJumpable;

    [Header("Combate")]
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackDamage;
    [SerializeField] private LayerMask whatIsDamageable;

    private Rigidbody2D rb;
    private float inputH;
    private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movimiento();

        Saltar();

        LanzarAtaque();
    }

    private void LanzarAtaque()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
        }
    }

    //Launched from anim event
    private void Atacar()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, whatIsDamageable);
        foreach (Collider2D collision in collisions) 
        {
            HealthSystem healthComponent = collision.gameObject.GetComponent<HealthSystem>();
            healthComponent.ReceiveDamage(attackDamage);
        }
    }

    private void Saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }

    private bool isGrounded()
    {
        return Physics2D.Raycast(feet.transform.position, Vector3.down, floorDetectionDistance, whatIsJumpable);
    }

    private void Movimiento()
    {
        inputH = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(inputH * velocidadMov, rb.linearVelocity.y);
        anim.SetBool("isRunning", inputH != 0);
        if (inputH != 0) transform.eulerAngles = inputH > 0 ? Vector3.zero : new Vector3(0, 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }
}
