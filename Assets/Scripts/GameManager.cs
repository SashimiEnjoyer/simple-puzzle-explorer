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

    public void OpenCardMatchPuzzle(UnityAction OnClosePuzzle, UnityAction OnPuzzleSolved)
    {
        if (currentState == GameState.Puzzle)
            return;

        gameManager.CurrentState = GameState.Puzzle;

        GameObject go = Instantiate(cardMatchPuzzle);
        go.GetComponent<CardMatchPuzzleManager>().InitPuzzle(() =>
        {
            gameManager.CurrentState = GameState.Play;
            OnClosePuzzle?.Invoke();
        }, () =>
        {
            gameManager.CurrentState = GameState.Play;
            OnPuzzleSolved?.Invoke();
        });
    }

    public void OpeSlideImagePuzzle(UnityAction OnClosePuzzle, UnityAction OnPuzzleSolved)
    {
        if (currentState == GameState.Puzzle)
            return;

        gameManager.CurrentState = GameState.Puzzle;

        GameObject go = Instantiate(slideImagePuzzle);
        go.GetComponent<SlidingPuzzle>().InitPuzzle(() =>
        {
            gameManager.CurrentState = GameState.Play;
            OnClosePuzzle?.Invoke();
        }, () =>
        {
            gameManager.CurrentState = GameState.Play;
            OnPuzzleSolved?.Invoke();
        });
    }

    public void OpeRotateLockPuzzle(UnityAction OnClosePuzzle, UnityAction OnPuzzleSolved)
    {
        if (currentState == GameState.Puzzle)
            return;

        gameManager.CurrentState = GameState.Puzzle;

        GameObject go = Instantiate(rotateLockPuzzle);
        go.GetComponent<RotateLockPuzzleManager>().InitPuzzle(() =>
        {
            gameManager.CurrentState = GameState.Play;
            OnClosePuzzle?.Invoke();
        }, () =>
        {
            gameManager.CurrentState = GameState.Play;
            OnPuzzleSolved?.Invoke();
        });
    }
}
