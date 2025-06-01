using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] bool unlockDash;
    [SerializeField] bool unlockAltAttack;
    [SerializeField] bool unlockSpecialAttack;

    private bool abierto = false;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0f;
    }

    public void Interact()
    {
        if (!abierto)
        {
            anim.speed = 1f;
            anim.Play("ChestOpen", 0, 0f);
            Debug.Log("¡Cofre abierto!");
            abierto = true;
            Destroy(gameObject.GetComponent<ObjectInfo>().text);

            GameObject.FindGameObjectWithTag("PlayerHitbox").GetComponent<Player>().fUnlockAltAttack = unlockAltAttack;
            GameObject.FindGameObjectWithTag("PlayerHitbox").GetComponent<Player>().fUnlockSpecialAttack = unlockSpecialAttack;
            GameObject.FindGameObjectWithTag("PlayerHitbox").GetComponent<Player>().fUnlockDash = unlockDash;
        }
    }
}
