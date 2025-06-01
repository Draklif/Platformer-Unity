using UnityEngine;

public class ObjectInfo : MonoBehaviour
{
    [SerializeField] public GameObject text;
    [SerializeField] public bool isEnabled = true;

    private void Update()
    {
        if (text != null) { text.SetActive(isEnabled); }
    }
}
