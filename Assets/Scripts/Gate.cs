using DG.Tweening;
using UnityEngine;

public class Gate : Interactable
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform[] gates;
    [SerializeField] private Vector3[] rotateTarget;

    protected override void ExecuteEvent()
    {
        base.ExecuteEvent();
        gameManager.OpeRotateLockPuzzle(()=>isAlreadyInteracted = false , OpenTheGate);
    }

    private void OpenTheGate()
    {
        for (int i = 0; i < gates.Length; i++)
        {
            gates[i].DORotate(rotateTarget[i], 0.5f);
        }

    }

}
