using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardEgara
{
    Spade,
    Heart,
    Club,
    Dia
}

public enum CardNumber
{
    Ace = 1,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}

public struct CardData
{
    private CardEgara egara;
    private CardNumber number;

    public CardData(CardEgara egara, CardNumber number)
    {
        this.egara = egara;
        this.number = number;
    }

    public CardEgara Egara
    {
        get
        {
            return egara;
        }
    }

    public CardNumber Number
    {
        get
        {
            return number;
        }
    }

    public override string ToString()
    {
        return egara + " : " + NumberString();
    }

    string NumberString()
    {
        if (number >= CardNumber.Two && number <= CardNumber.Ten)
            return ((int)number).ToString();

        return number.ToString().Substring(0, 1);
    }
}

public class Card : EventTrigger
{
    private bool isReverse;
    private CardData data;
    private Sprite cardSpr;
    private CardMove move;

    public bool IsReverse
    {
        get
        {
            return isReverse;
        }
    }

    public CardData Data
    {
        get
        {
            return data;
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!GOIS.EventButton(eventData))
            return;

        if (!isReverse)
            move.OnDrag(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!GOIS.EventButton(eventData))
            return;

        if (!isReverse)
        {
            move.OnPointerDown(eventData, transform);

            return;
        }

        if (transform.GetSiblingIndex() == transform.parent.childCount - 1)
            OpenCard();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!GOIS.EventButton(eventData))
            return;

        if (!isReverse)
            move.OnPointerUp(eventData);
    }

    public void SetCard(CardData card, CardSprites sprites, CardMove move, bool isReverse)
    {
        this.isReverse = isReverse;
        data = card;
        cardSpr = sprites.GetSprite(card);
        this.move = move;

        if (!isReverse)
            OpenCard();
    }

    void OpenCard()
    {
        isReverse = false;

        GetComponent<UnityEngine.UI.Image>().sprite = cardSpr;
    }

    public void DebugOihagi()
    {
        OpenCard();
    }

    public RectTransform GetRectTransform()
    {
        return (RectTransform)transform;
    }

    public IEnumerator Move(Vector3 target, float speed, Transform parent)
    {
        yield return move.Move(transform, target, speed, parent);
    }
}
