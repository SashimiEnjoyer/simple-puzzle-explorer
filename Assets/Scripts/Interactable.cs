using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObject interactableIndicator;
    [SerializeField] protected bool interactOnce;
    [SerializeField] protected UnityEvent OnTouch;

    protected bool isAlreadyInteracted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (interactOnce && isAlreadyInteracted)
            return;

        if (other.CompareTag("Player"))
        {
            if (interactableIndicator)
                interactableIndicator.SetActive(false);

            ExecuteEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactOnce && isAlreadyInteracted)
            return;

        if (other.CompareTag("Player"))
        {
            if (interactableIndicator)
                interactableIndicator.SetActive(true);
        }
    }

    protected virtual void ExecuteEvent()
    {
        OnTouch?.Invoke();
        isAlreadyInteracted = true;
    }
}
