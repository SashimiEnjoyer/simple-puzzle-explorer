using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject cnPauseMenu;
    [SerializeField] private GameObject cnInteractIndicator;

    private void Awake()
    {
        gameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        gameManager.OnGameStateChanged -= OnGameStateChanged;
    }


    private void OnGameStateChanged(GameState state)
    {
        ShowPauseMenu(state == GameState.Pause);
    }

    public void ShowPauseMenu(bool state)
    {
        cnPauseMenu.SetActive(state);
    }

    public void ShowInteractIndicator(bool state)
    {
        Debug.Log($"ShowInteractIndicator: {state}");
        cnInteractIndicator.SetActive(state);
    }
}
