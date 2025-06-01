using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private bool abierto = false;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0f; // Reanuda la animación
    }

    public void Interact()
    {
        if (!abierto)
        {
            anim.speed = 1f; // Reanuda la animación
            anim.Play("ChestOpen", 0, 0f); // Reproduce desde el inicio si hace falta
            Debug.Log("¡Cofre abierto!");
            abierto = true;
            Destroy(gameObject.GetComponent<ObjectInfo>().text);
        }
    }
}
