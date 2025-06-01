using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private GameObject feet;
    [SerializeField] private float velocidadMov;
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private float floorDetectionDistance;
    [SerializeField] private LayerMask whatIsJumpable;

    [Header("Salto múltiple")]
    [SerializeField] private int maxSaltos = 2;
    private int saltosRestantes;

    [Header("Combate")]
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackDamageFireball;
    [SerializeField] private LayerMask whatIsDamageable;

    private bool isSpecialAttacking = false;

    [Header("Dash")]
    [SerializeField] private GameObject dashSmoke;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing = false;
    private bool canDash = true;

    private Rigidbody2D rb;
    private float inputH;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        saltosRestantes = maxSaltos;
    }

    void Update()
    {
        Movimiento();

        Saltar();

        LanzarAtaque();
        LanzarAtaqueBolaFuego();
        LanzarAtaqueEspecial();

        Dash();
    }

    private void LanzarAtaque()
    {
        if (Input.GetMouseButtonDown(0) && !isSpecialAttacking)
        {
            anim.SetTrigger("attack");
        }
    }

    private void LanzarAtaqueBolaFuego()
    {
        if (Input.GetMouseButtonDown(1) && !isSpecialAttacking)
        {
            anim.SetTrigger("attack_fire");
        }
    }

    private void LanzarAtaqueEspecial()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isSpecialAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetTrigger("attack_special");
            isSpecialAttacking = true;
            HealthSystem healthComponent = gameObject.GetComponent<HealthSystem>();
            healthComponent.isInvulnerable = true;
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

    //Launched from anim event
    private void AtacarBolaFuego()
    {
        GameObject fireball = Instantiate(fireballPrefab, attackPoint.transform.position, Quaternion.identity);

        float direction = transform.eulerAngles.y == 0 ? 1f : -1f;
        fireball.GetComponent<FireBall>().Disparar(direction);
        fireball.GetComponent<FireBall>().attackDamage = attackDamageFireball;
        fireball.GetComponent<FireBall>().isFriendly = true;
    }

    //Launched from anim event
    private void AtacarEspecial()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, whatIsDamageable);
        foreach (Collider2D collision in collisions)
        {
            HealthSystem healthComponent = collision.gameObject.GetComponent<HealthSystem>();
            healthComponent.ReceiveDamage(attackDamage * 2);
        }
    }

    //Launched from anim event
    private void FinalizarEspecial()
    {
        isSpecialAttacking = false;
        HealthSystem healthComponent = gameObject.GetComponent<HealthSystem>();
        healthComponent.isInvulnerable = false;
    }

    private void Saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
            saltosRestantes--;
        }

        if (isGrounded())
        {
            saltosRestantes = maxSaltos;
        }
    }

    private void Interactuar()
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
        if (isDashing || isSpecialAttacking) return;

        inputH = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(inputH * velocidadMov, rb.linearVelocity.y);
        anim.SetBool("isRunning", inputH != 0);
        if (inputH != 0) transform.eulerAngles = inputH > 0 ? Vector3.zero : new Vector3(0, 180, 0);
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing && !isSpecialAttacking)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        canDash = false;

        HealthSystem healthComponent = gameObject.GetComponent<HealthSystem>();
        healthComponent.isInvulnerable = true;
        dashSmoke.SetActive(true);

        float dashDirection = transform.eulerAngles.y == 0 ? 1f : -1f;
        rb.linearVelocity = new Vector2(dashDirection * dashForce, rb.linearVelocity.y);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        healthComponent.isInvulnerable = false;

        yield return new WaitForSeconds(dashCooldown);

        Debug.Log("we can dash now");
        canDash = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }
}
