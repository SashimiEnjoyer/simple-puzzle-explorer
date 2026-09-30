using DG.Tweening;
using System;
using UnityEngine;

public class PuzzleGameTrigger : Interactable
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PuzzleType type;

    protected override void ExecuteEvent()
    {
        base.ExecuteEvent();

        switch (type)
        {
            case PuzzleType.CardMatch:
                gameManager.OpenCardMatchPuzzle(() => isAlreadyInteracted = false, null);
                break;
            case PuzzleType.ImageSlide:
                gameManager.OpeSlideImagePuzzle(() => isAlreadyInteracted = false, null);
                break;
            case PuzzleType.RotateLock:
                gameManager.OpeRotateLockPuzzle(() => isAlreadyInteracted = false, null);
                break;
            default:
                break;
        }


    }
}
