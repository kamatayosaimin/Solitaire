using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardPutter : MonoBehaviour
{
    public abstract void SetPosition(float distance, float limit);
    protected abstract bool PutEgaraCheck(Card card, Card other);
    protected abstract bool PutNumberCheck(Card card, Card other);

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool IsEmpty()
    {
        return transform.childCount == 0;
    }

    public bool PutCardCheck(Card other)
    {
        Card card = GetTopCard();

        return PutEgaraCheck(card, other) && PutNumberCheck(card, other);
    }

    public Card GetTopCard()
    {
        return GetCard(transform.childCount - 1);
    }

    protected Card GetCard(int index)
    {
        try
        {
            return transform.GetChild(index).GetComponent<Card>();
        }
        catch (UnityException e)
        {
            return null;
        }
    }
}
