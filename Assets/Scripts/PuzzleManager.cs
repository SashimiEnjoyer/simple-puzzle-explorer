using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private Button CloseBtn;
    [SerializeField] private Button FinishBtn;

    protected UnityAction OnPuzzleFinish;
    protected UnityAction OnPuzzleClose;


    private void Awake()
    {
        CloseBtn.onClick.AddListener(() =>
        {
            CloseAndDestroyPuzzle();
            OnPuzzleClose?.Invoke();
        });

        FinishBtn.onClick.AddListener(() =>
        {
            CloseAndDestroyPuzzle();
            OnPuzzleFinish?.Invoke();
        });

        SetFinishBtnActiveState(false);
    }

    public virtual void InitPuzzle(UnityAction closeEvent, UnityAction finishedEvent)
    {
        OnPuzzleClose = closeEvent;
        OnPuzzleFinish = finishedEvent;
    }

    protected void CloseAndDestroyPuzzle()
    {
        Destroy(gameObject);
    }

    protected void SetFinishBtnActiveState(bool state)
    {
        FinishBtn.gameObject.SetActive(state);
    }
}
