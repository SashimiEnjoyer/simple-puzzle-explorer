using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardMatchPuzzleManager : PuzzleManager
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardsParent;
    [SerializeField] private GameObject checkCardIndicatorObj;

    [SerializeField] private CardMatchData[] caradDatas;
    private List<CardPuzzleEntity> deck = new();
    private CardPuzzleEntity tempCard;

    private int solvedCounter = 0;

    public override void InitPuzzle(UnityAction closeEvent, UnityAction finishedEvent)
    {
        base.InitPuzzle(closeEvent, finishedEvent);

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
        seq.AppendInterval(0.75f);
        seq.AppendCallback(() =>
        {
            if (tempCard.CardData.type == card.CardData.type)
            {
                tempCard.SetCardSolved();
                card.SetCardSolved();
                solvedCounter++;

                if (solvedCounter > caradDatas.Length - 1)
                {
                    SetFinishBtnActiveState(true);
                }
            }
            else
            {
                tempCard.SetCardOpenState(false);
                card.SetCardOpenState(false);
            }
            tempCard = null;
        });
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() => checkCardIndicatorObj.SetActive(false));
    }
}