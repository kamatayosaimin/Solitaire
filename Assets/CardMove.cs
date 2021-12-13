using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMove : MonoBehaviour
{
    private Transform fromParent;
    private Play main;

    // Use this for initialization
    void Start()
    {
        main = GetComponentInParent<Play>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void InitPosition()
    {
        transform.localPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMousePosition(eventData);
    }

    public void OnPointerDown(PointerEventData eventData, Transform touchedCard)
    {
        fromParent = touchedCard.parent;

        transform.position = GetMousePosition(eventData);

        ChangeParent(fromParent, transform, touchedCard.GetSiblingIndex());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (fromParent)
            ChangeHolder(eventData);

        InitPosition();

        main.SetPosition();
    }

    void ChangeParent(Transform from, Transform to, int index)
    {
        int count = from.childCount;

        for (int i = 0; i < count - index; i++)
            from.GetChild(index).SetParent(to);
    }

    void ChangeHolder(PointerEventData eventData)
    {
        OnDrag(eventData);

        Transform toParent = main.GetToParent(fromParent);

        if (!toParent)
        {
            toParent = NotChangeTo();

            if (toParent == fromParent)
                main.ClearCheck();
        }

        ChangeParent(transform, toParent, 0);

        fromParent = null;
    }

    Vector3 GetMousePosition(PointerEventData eventData)
    {
        Vector3 local = GetMouseLocalPosition(eventData);

        return main.transform.TransformPoint(local);
    }

    Vector3 GetMouseLocalPosition(PointerEventData eventData)
    {
        Vector3 origin = GetMouseOrigin(eventData),
            size = main.SizeDelta;

        return Vector3.Scale(origin, size);
    }

    Vector3 GetMouseOrigin(PointerEventData eventData)
    {
        Vector3 point = Camera.main.ScreenToViewportPoint(eventData.position),
            offset = new Vector3(1f, 1f, 0f) / 2f;

        return point - offset;
    }

    Transform NotChangeTo()
    {
        return transform.childCount != 1 ? fromParent : ChangeToPost();
    }

    Transform ChangeToPost()
    {
        Card moveCard = GetBottomCard();
        CardPost[] posts = main.GetCardPosts();

        foreach (var p in posts)
            if (p.PutCardCheck(moveCard))
                return p.transform;

        return fromParent;
    }

    public Card GetBottomCard()
    {
        if (transform.childCount == 0)
            return null;

        return transform.GetChild(0).GetComponent<Card>();
    }

    public IEnumerator Move(Transform card, Vector3 target, float speed, Transform parent)
    {
        transform.position = card.position;

        card.SetParent(transform);

        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            yield return null;
        }

        card.SetParent(parent);

        InitPosition();
    }
}
