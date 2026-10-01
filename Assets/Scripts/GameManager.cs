using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public enum GameState { Play, Puzzle, Pause}

public class GameManager : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs input;
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

    private void Awake()
    {
        input.OnPausePressed += () =>
        {
            if (currentState == GameState.Pause)
                CurrentState = GameState.Play;
            else if (currentState == GameState.Play)
                CurrentState = GameState.Pause;
        };
    }

    public void OpenCardMatchPuzzle(UnityAction OnClosePuzzle, UnityAction OnPuzzleSolved)
    {
        if (currentState == GameState.Puzzle)
            return;

        CurrentState = GameState.Puzzle;

        GameObject go = Instantiate(cardMatchPuzzle);
        go.GetComponent<CardMatchPuzzleManager>().InitPuzzle(() =>
        {
            CurrentState = GameState.Play;
            OnClosePuzzle?.Invoke();
        }, () =>
        {
            CurrentState = GameState.Play;
            OnPuzzleSolved?.Invoke();
        });
    }

    public void OpeSlideImagePuzzle(UnityAction OnClosePuzzle, UnityAction OnPuzzleSolved)
    {
        if (currentState == GameState.Puzzle)
            return;

        CurrentState = GameState.Puzzle;

        GameObject go = Instantiate(slideImagePuzzle);
        go.GetComponent<SlidingPuzzle>().InitPuzzle(() =>
        {
            CurrentState = GameState.Play;
            OnClosePuzzle?.Invoke();
        }, () =>
        {
            CurrentState = GameState.Play;
            OnPuzzleSolved?.Invoke();
        });
    }

    public void OpeRotateLockPuzzle(UnityAction OnClosePuzzle, UnityAction OnPuzzleSolved)
    {
        if (currentState == GameState.Puzzle)
            return;

        CurrentState = GameState.Puzzle;

        GameObject go = Instantiate(rotateLockPuzzle);
        go.GetComponent<RotateLockPuzzleManager>().InitPuzzle(() =>
        {
            CurrentState = GameState.Play;
            OnClosePuzzle?.Invoke();
        }, () =>
        {
            CurrentState = GameState.Play;
            OnPuzzleSolved?.Invoke();
        });
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
