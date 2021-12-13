using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : CardPutter
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetCard(List<CardData> cards, CardSprites sprites, CardMove move)
    {
        foreach (Transform c in transform)
        {
            CardData card = cards[Random.Range(0, cards.Count)];

            c.GetComponent<Card>().SetCard(card, sprites, move, transform.childCount - c.GetSiblingIndex() > 3);

            cards.Remove(card);
        }
    }

    public override void SetPosition(float distance, float limit)
    {
        if (IsEmpty())
            return;

        float offset = 0f, _distance = GetDistance(distance, limit), half = distance / 2f;

        foreach (Transform c in transform)
        {
            c.localPosition = Vector3.down * offset;

            offset += c.GetComponent<Card>().IsReverse ? half : _distance;
        }
    }

    float GetDistance(float distance, float limit)
    {
        int reverseCount = 0;
        float total = 0f;

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            float add = distance;

            if (GetCard(i).IsReverse)
            {
                reverseCount++;
                add /= 2f;
            }

            total += add;
        }

        return total > limit ? GetDistance(distance, limit, reverseCount) : distance;
    }

    float GetDistance(float distance, float limit, int reverseCount)
    {
        int count = transform.childCount - reverseCount - 1;
        float length = limit - distance / 2f * reverseCount;

        return length / count;
    }

    protected override bool PutEgaraCheck(Card card, Card other)
    {
        if (!card)
            return true;

        return IsBlack(card) != IsBlack(other);
    }

    protected override bool PutNumberCheck(Card card, Card other)
    {
        CardNumber otherNumber = other.Data.Number;

        if (!card)
            return otherNumber == CardNumber.King;

        return card.Data.Number - otherNumber == 1;
    }

    bool IsBlack(Card card)
    {
        CardEgara egara = card.Data.Egara;

        return egara == CardEgara.Spade || egara == CardEgara.Club;
    }

    public bool IsCollision(Card dragCard, Transform fromParent)
    {
        bool isOther = transform != fromParent;
        RectTransform dragPoint = dragCard.GetRectTransform();

        return isOther && IsCollision(dragPoint) && PutCardCheck(dragCard);
    }

    bool IsCollision(RectTransform dragPoint)
    {
        bool[] value = new bool[2];
        RectTransform point = CollisionPoint();

        Vector3 distance = CollisionDistance(point, dragPoint),
            size = CollisionSize(point, dragPoint);

        for (int i = 0; i < value.Length; i++)
            value[i] = IsCollisionValue(distance[i], size[i]);

        return GOISBooleanArray.And(value);
    }

    bool IsCollisionValue(float distance, float size)
    {
        return Mathf.Abs(distance) < size;
    }

    Vector3 CollisionDistance(Transform point, Transform dragPoint)
    {
        return GetPosition(point) - GetPosition(dragPoint);
    }

    Vector3 CollisionSize(RectTransform point, RectTransform dragPoint)
    {
        Vector3 baseSize = point.sizeDelta + dragPoint.sizeDelta;

        return baseSize / 2f;
    }

    Vector3 GetPosition(Transform point)
    {
        return GetComponentInParent<Play>().transform.InverseTransformPoint(point.position);
    }

    RectTransform CollisionPoint()
    {
        Card topCard = GetTopCard();

        return topCard ? topCard.GetRectTransform() : (RectTransform)transform;
    }
}
