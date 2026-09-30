using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LockPuzzleEntity : MonoBehaviour
{
    [SerializeField] private Transform lockVisual;
    [SerializeField] private Button lockBtn;
    private UnityAction OnVisualRotated;

    private int startPos;
    private int currentPos;
    private int targetPos;
    public bool isSolved; 

    public void InitEntity(int start, int target, UnityAction OnButtonPressed)
    {
        startPos = start;
        currentPos = startPos;
        targetPos = target;
        OnVisualRotated = OnButtonPressed;

        lockBtn.onClick.AddListener(RotateLock);

        ResetEntity();
    }

    public void ResetEntity()
    {
        lockVisual.Rotate(Vector3.forward * (-45f * startPos));
        Check();

        OnVisualRotated?.Invoke();
    }

    public void RotateLock()
    {
        currentPos++;
        currentPos %= 8;

        lockVisual.Rotate(Vector3.forward * -45f);
        Check();

        OnVisualRotated?.Invoke();
    }

    private void Check()
    {
        if (currentPos == targetPos)
        {
            if (!isSolved)
            {
                isSolved = true;
            }
        }
        else
        {
            if (isSolved)
            {
                isSolved = false;
            }
        }
    }
}
