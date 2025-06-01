using UnityEngine;

public class FireBall : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float impulsoDisparo;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * impulsoDisparo, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
