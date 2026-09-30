using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject interactableIndicator;
    [SerializeField] private bool interactOnce;
    [SerializeField] private UnityEvent OnTouch;

    private bool isAlreadyInteracted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (interactOnce && isAlreadyInteracted)
            return;

        if (other.CompareTag("Player"))
        {
            interactableIndicator.SetActive(false);
            OnTouch?.Invoke();
            isAlreadyInteracted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!interactOnce)
            return;

        if (other.CompareTag("Player"))
        {
            interactableIndicator.SetActive(true);
        }
    }
}
