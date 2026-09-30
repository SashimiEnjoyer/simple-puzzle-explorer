using UnityEngine;
using UnityEngine.Events;

public enum GameState { Play, Puzzle}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject cardMatchPuzzle;
    [SerializeField] private GameObject slideImagePuzzle;
    [SerializeField] private GameObject rotateLockPuzzle;
    [SerializeField] private GameState currentState;

    public GameState CurrentState
    {
        get {return currentState; }
        set
        {
            if(value != currentState)
            {
                currentState = value;
                OnGameStateChanged?.Invoke(currentState);
            }
        }
    }
    public UnityAction<GameState> OnGameStateChanged;

    [ContextMenu("Open Card Puzzle")]
    public void OpenCardMatchPuzzle()
    {
        if (currentState == GameState.Puzzle)
            return;

        gameManager.CurrentState = GameState.Puzzle;

        GameObject go = Instantiate(cardMatchPuzzle);
        go.GetComponent<CardMatchPuzzleManager>().InitPuzzle(() =>
        {
            gameManager.CurrentState = GameState.Play;
        }, () =>
        {
            gameManager.CurrentState = GameState.Play;
        });
    }

    [ContextMenu("Open Sliding Image Puzzle")]
    public void OpeSlideImagePuzzle()
    {
        if (currentState == GameState.Puzzle)
            return;

        gameManager.CurrentState = GameState.Puzzle;

        GameObject go = Instantiate(slideImagePuzzle);
        go.GetComponent<SlidingPuzzle>().InitPuzzle(() =>
        {
            gameManager.CurrentState = GameState.Play;
        }, () =>
        {
            gameManager.CurrentState = GameState.Play;
        });
    }

    [ContextMenu("Open Lock Rotate Puzzle")]
    public void OpeRotateLockPuzzle()
    {
        if (currentState == GameState.Puzzle)
            return;

        gameManager.CurrentState = GameState.Puzzle;

        GameObject go = Instantiate(rotateLockPuzzle);
        go.GetComponent<RotateLockPuzzleManager>().InitPuzzle(() =>
        {
            gameManager.CurrentState = GameState.Play;
        }, () =>
        {
            gameManager.CurrentState = GameState.Play;
        });
    }

}
