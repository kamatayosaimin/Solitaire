using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPost : CardPutter
{
    [SerializeField]
    private CardEgara egara;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void SetPosition(float distance, float limit)
    {
        foreach (Transform c in transform)
            c.localPosition = Vector3.zero;
    }

    protected override bool PutEgaraCheck(Card card, Card other)
    {
        return other.Data.Egara == egara;
    }

    protected override bool PutNumberCheck(Card card, Card other)
    {
        CardNumber otherNumber = other.Data.Number;

        if (!card)
            return otherNumber == CardNumber.Ace;

        return otherNumber - card.Data.Number == 1;
    }

    bool OutOfFind(Card topCard)
    {
        return !topCard || topCard.IsReverse;
    }

    bool IsFindHit(CardNumber target, Card topCard)
    {
        CardData data = topCard.Data;

        return data.Egara == egara && data.Number == target;
    }

    public CardNumber GetTargetNumber()
    {
        Card top = GetTopCard();

        return top ? top.Data.Number + 1 : CardNumber.Ace;
    }

    public MovingCard FindFinishCard(CardNumber target)
    {
        if (GetTargetNumber() != target)
            return null;

        foreach (var h in GetComponentInParent<Play>().GetCardHolders())
        {
            MovingCard find = FindFinishCard(target, h);

            if (find)
                return find;
        }

        return null;
    }

    MovingCard FindFinishCard(CardNumber target, CardHolder holder)
    {
        Card top = holder.GetTopCard();

        return OutOfFind(top) ? null : FindFinishCard(target, top);
    }

    MovingCard FindFinishCard(CardNumber target, Card topCard)
    {
        return IsFindHit(target, topCard) ? FindFinishCard(topCard) : null;
    }

    MovingCard FindFinishCard(Card topCard)
    {
        return new MovingCard(topCard, transform.position, transform);
    }
}
