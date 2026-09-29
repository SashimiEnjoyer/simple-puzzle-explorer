using UnityEngine;
using UnityEngine.Events;

public enum GameState { Play, Puzzle}

public class GameManager : MonoBehaviour
{
    public UnityAction<GameState> OnGameStateChanged;
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
    

}
