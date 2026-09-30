using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RotateLockPuzzleManager : PuzzleManager
{

    [System.Serializable]
    public class LockPuzzleData
    {
        public LockPuzzleEntity lockEntity;
        public int startingPos;
        public int targetPos;
    }

    [SerializeField] private LockPuzzleData[] lockPuzzles;
    [SerializeField] private int solvedCount;

    private Vector3 cachedRotation;
    [SerializeField] private bool isAllSolved;

    [SerializeField] private List<LockPuzzleData> lockPuzzleList = new();

    public UnityAction OnLockPuzzleSolved;

    public override void InitPuzzle(UnityAction closeEvent, UnityAction finishedEvent)
    {
        base.InitPuzzle(closeEvent, finishedEvent);

        for (int i = 0; i < lockPuzzles.Length; i++)
        {
            int idx = i;
            lockPuzzleList.Add(lockPuzzles[idx]);
            lockPuzzleList[i].lockEntity.InitEntity(lockPuzzleList[i].startingPos, lockPuzzleList[i].targetPos, CheckPuzzle);
        }
    }

    private void ResetPuzzle()
    {
        for (int i = 0; i < lockPuzzleList.Count; i++)
        {
            int idx = i;
            lockPuzzleList[i].lockEntity.ResetEntity();
            
        }
    }

    private void CheckPuzzle()
    {
        isAllSolved = true;

        for (int i = 0; i < lockPuzzleList.Count; i++)
        {
            if(!lockPuzzleList[i].lockEntity.isSolved)
            {
                isAllSolved = false;
                return;
            }
        }

        SetFinishBtnActiveState(true);
    }
}
