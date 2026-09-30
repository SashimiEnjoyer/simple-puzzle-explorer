using DG.Tweening;
using UnityEngine;

public class Gate : Interactable
{
    [SerializeField] private bool usePuzzle;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform[] gates;
    [SerializeField] private Vector3[] rotateTarget;

    protected override void ExecuteEvent()
    {
        base.ExecuteEvent();

        if (usePuzzle)
            gameManager.OpeRotateLockPuzzle(() => isAlreadyInteracted = false, OpenTheGate);
        else
            OpenTheGate();
    }

    private void OpenTheGate()
    {
        for (int i = 0; i < gates.Length; i++)
        {
            gates[i].DOLocalRotate(rotateTarget[i], 0.5f);
        }

    }

}
