using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject cardMatchPuzzle;
    [SerializeField] private GameObject slideImagePuzzle;

    [ContextMenu("Open Card Puzzle")]
    public void OpenCardMatchPuzzle()
    {
        gameManager.CurrentState = GameState.Puzzle;

        GameObject go = Instantiate(cardMatchPuzzle);
        go.GetComponent<CardMatchPuzzleManager>().InitCardMatchingPuzzle(null, () =>
        {
            gameManager.CurrentState = GameState.Play;
        });
    }

    [ContextMenu("Open Sliding Image Puzzle")]
    public void OpeSlideImagePuzzle()
    {
        gameManager.CurrentState = GameState.Puzzle;

        GameObject go = Instantiate(slideImagePuzzle);
        go.GetComponent<SlidingPuzzle>().InitSlidingPuzzle(null, () =>
        {
            gameManager.CurrentState = GameState.Play;
        });
    }
}
