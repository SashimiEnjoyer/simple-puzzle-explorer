using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardMatchPuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardsParent;
    [SerializeField] private GameObject checkCardIndicatorObj;
    [SerializeField] private Button CloseBtn;
    [SerializeField] private Button FinishBtn;
    [SerializeField] private CardMatchData[] caradDatas;
    private List<CardPuzzleEntity> deck = new();
    private CardPuzzleEntity tempCard;
    private UnityAction OnPuzzleFinish;
    private UnityAction OnPuzzleClose;
    private int solvedCounter = 0;

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
    }

    public void InitCardMatchingPuzzle(UnityAction onClose, UnityAction onFinish)
    {
        OnPuzzleClose = onClose;
        OnPuzzleFinish = onFinish;

        FinishBtn.gameObject.SetActive(false);

        List<CardMatchData> dataPool = new();
        foreach (var data in caradDatas)
        {
            dataPool.Add(data);
            dataPool.Add(data);
        }

        Shuffle(dataPool);

        foreach (var data in dataPool)
        {
            GameObject go = Instantiate(cardPrefab, cardsParent);
            CardPuzzleEntity c = go.GetComponent<CardPuzzleEntity>();
            c.InitCardEntity(data, CheckCard);
            deck.Add(c);
        }
    }

    //private void Start()
    //{
    //    InitCardMatchingPuzzle(CloseAndDestroyPuzzle, CloseAndDestroyPuzzle);
    //}

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private void CheckCard(CardPuzzleEntity card)
    {
        if(tempCard == null)
        {
            tempCard = card;
        }
        else
        {
            SequenceCheckCard(card);
        }
    }

    private void SequenceCheckCard(CardPuzzleEntity card)
    {
        checkCardIndicatorObj.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() =>
        {
            if (tempCard.CardData.type == card.CardData.type)
            {
                tempCard.SetCardSolved();
                card.SetCardSolved();
                solvedCounter++;

                if (solvedCounter > caradDatas.Length - 1)
                {
                    FinishBtn.gameObject.SetActive(true);
                }
            }
            else
            {
                tempCard.SetCardOpenState(false);
                card.SetCardOpenState(false);
            }
            tempCard = null;
        });
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() => checkCardIndicatorObj.SetActive(false));
    }

    private void CloseAndDestroyPuzzle()
    {
        Destroy(gameObject);
    }
}