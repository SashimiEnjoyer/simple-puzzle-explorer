using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class CardMatchData
{
    public int type;
    public Sprite img;
}

public class CardPuzzleEntity : MonoBehaviour
{
    [SerializeField] private Button card;
    [SerializeField] private GameObject cardVisualObj;
    [SerializeField] private Image cardImg;
    private bool isSolved = false;

    private CardMatchData cardData;
    public CardMatchData CardData => cardData;

    public void InitCardEntity(CardMatchData data, UnityAction<CardPuzzleEntity> OnCardCheck)
    {
        cardData = data;
        cardImg.sprite = cardData.img;
        SetCardOpenState(false);
        card.onClick.AddListener(() =>
        {
            if (isSolved)
                return;

            SetCardOpenState(true);
            OnCardCheck?.Invoke(this);
        });
    }

    public void SetCardOpenState(bool state)
    {
        cardImg.gameObject.SetActive(state);
    }

    public void SetCardSolved()
    {
        isSolved = true;
        cardVisualObj.SetActive(false);
    }
}
