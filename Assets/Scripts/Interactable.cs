using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected StarterAssetsInputs input;
    [SerializeField] protected UiManager uiManager;
    [SerializeField] protected GameObject interactableIndicator;
    [SerializeField] protected bool interactOnce;
    [SerializeField] protected bool useOnTrigger;
    [SerializeField] protected UnityEvent OnTouch;

    protected bool isAlreadyInteracted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (interactOnce && isAlreadyInteracted)
            return;

        if (other.CompareTag("Player"))
        {
            if (!useOnTrigger)
            {
                input.OnInteractPressed = ExecuteEvent;
            }

            SetIndicatorUi(true);

            if (!useOnTrigger)
                return;

            ExecuteEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactOnce && isAlreadyInteracted)
            return;

        if (other.CompareTag("Player"))
        {
            if (!useOnTrigger)
            {
                input.OnInteractPressed = null;
            }

            SetIndicatorUi(false);
        }
    }

    protected void SetIndicatorUi(bool state)
    {
        uiManager.ShowInteractIndicator(state);
    }

    protected virtual void ExecuteEvent()
    {
        OnTouch?.Invoke();
        isAlreadyInteracted = true;
    }
}
