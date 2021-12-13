using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CardSprites
{
    private const int Count = (int)CardNumber.King;
    [SerializeField]
    private Sprite[] spade = new Sprite[Count],
        heart = new Sprite[Count],
        club = new Sprite[Count],
        dia = new Sprite[Count];

    public Sprite GetSprite(CardData card)
    {
        Dictionary<CardEgara, Sprite[]> sprites = new Dictionary<CardEgara, Sprite[]>()
        {
            {
                CardEgara.Spade,
                spade
            },
            {
                CardEgara.Heart,
                heart
            },
            {
                CardEgara.Club,
                club
            },
            {
                CardEgara.Dia,
                dia
            }
        };

        return sprites[card.Egara][(int)card.Number - 1];
    }
}

public class MovingCard
{
    private Vector3 target;
    private Transform parent;
    private Card card;

    public MovingCard(Card card, Vector3 target, Transform parent)
    {
        this.target = target;
        this.parent = parent;
        this.card = card;
    }

    public static implicit operator bool(MovingCard card)
    {
        return card != null;
    }

    public IEnumerator Move(float speed)
    {
        yield return card.Move(target, speed, parent);
    }
}

public class Play : GOISSceneManager
{
    [SerializeField]
    private float cardDistance, limitDistance, cardSpeed, finishEndSpan;
    [SerializeField]
    private string nextScene;
    [SerializeField]
    private CanvasGroup field;
    [SerializeField]
    private GameObject mainUI, clear;
    private CardMove move;
    [SerializeField]
    private CardSprites sprites;

    public Vector2 SizeDelta
    {
        get
        {
            return GetComponent<RectTransform>().sizeDelta;
        }
    }

    // Use this for initialization
    void Start()
    {
        move = GetComponentInChildren<CardMove>();

        List<CardData> cards = new List<CardData>();

        for (CardEgara e = CardEgara.Spade; e <= CardEgara.Dia; e++)
            for (CardNumber n = CardNumber.Ace; n <= CardNumber.King; n++)
                cards.Add(new CardData(e, n));

        foreach (var h in GetCardHolders())
            h.SetCard(cards, sprites, move);

        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            DebugOihagi();
    }

    public void OnTitleButton()
    {
        Next(nextScene);
    }

    public void OnResetButton()
    {
        Reload();
    }

    public void OnFinishButton()
    {
        StartCoroutine(Finish());
    }

    void SetActive(bool value)
    {
        field.blocksRaycasts = value;

        mainUI.SetActive(value);
    }

    public void SetPosition()
    {
        foreach (var h in GetComponentsInChildren<CardPutter>())
            h.SetPosition(cardDistance, limitDistance);
    }

    public void ClearCheck()
    {
        if (!IsClear())
            return;

        SetActive(false);

        clear.SetActive(true);
    }

    void DebugOihagi()
    {
        foreach (var c in GetComponentsInChildren<Card>())
            c.DebugOihagi();
    }

    float CollisionDistance(Transform dragCard, Transform collisionCard)
    {
        return Vector3.Distance(dragCard.position, collisionCard.position);
    }

    bool IsClear()
    {
        foreach (var h in GetCardHolders())
            if (!h.IsEmpty())
                return false;

        return true;
    }

    public Transform GetToParent(Transform fromParent)
    {
        Card dragCard = move.GetBottomCard();
        CardHolder[] collisionHolders = GetCollisionHolders(dragCard, fromParent);

        return collisionHolders.Length != 0 ? GetToParent(collisionHolders, dragCard) : null;
    }

    Transform GetToParent(CardHolder[] collisionHolders, Card dragCard)
    {
        Transform dragPoint = dragCard.transform,
            shortest = collisionHolders[0].transform;

        float minDistance = CollisionDistance(dragPoint, shortest);

        for (int i = 1; i < collisionHolders.Length; i++)
        {
            Transform collisionCard = collisionHolders[i].transform;

            float distance = CollisionDistance(dragPoint, collisionCard);

            if (distance < minDistance)
            {
                minDistance = distance;
                shortest = collisionCard;
            }
        }

        return shortest;
    }

    public CardHolder[] GetCardHolders()
    {
        return GetComponentsInChildren<CardHolder>();
    }

    CardHolder[] GetCollisionHolders(Card dragCard, Transform fromParent)
    {
        return GetCollisionHoldersCollection(dragCard, fromParent).ToArray();
    }

    public CardPost[] GetCardPosts()
    {
        return GetComponentsInChildren<CardPost>();
    }

    MovingCard FindFinishCard()
    {
        CardNumber target = GetCardPosts().Min(p => p.GetTargetNumber());

        foreach (var p in GetCardPosts())
        {
            MovingCard finish = p.FindFinishCard(target);

            if (finish)
                return finish;
        }

        return null;
    }

    IEnumerable<CardHolder> GetCollisionHoldersCollection(Card dragCard, Transform fromParent)
    {
        return GetCardHolders().Where(h => h.IsCollision(dragCard, fromParent));
    }

    IEnumerator Finish()
    {
        SetActive(false);

        while (true)
        {
            MovingCard finish = FindFinishCard();

            if (!finish)
                break;

            yield return finish.Move(cardSpeed);
        }

        yield return new WaitForSeconds(finishEndSpan);

        SetActive(true);

        ClearCheck();
    }
}
