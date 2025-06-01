using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable objetoCercano = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            other.GetComponent<ObjectInfo>().isEnabled = true;
            objetoCercano = other.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            other.GetComponent<ObjectInfo>().isEnabled = false;
            objetoCercano = null;
        }
    }

    void Update()
    {
        if (objetoCercano != null && Input.GetKeyDown(KeyCode.E))
        {
            objetoCercano.Interact();
        }
    }
}
