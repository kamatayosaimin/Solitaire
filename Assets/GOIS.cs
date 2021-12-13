using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class GOIS
{

    public static bool ButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public static bool EventButton(PointerEventData eventData)
    {
        return eventData.button == PointerEventData.InputButton.Left;
    }
}

public static class GOISBooleanArray
{

    public static bool And(bool[] value)
    {
        return GetValue(value, true);
    }

    public static bool Or(bool[] value)
    {
        return GetValue(value, false);
    }

    static bool GetValue(bool[] value, bool isAnd)
    {
        foreach (var v in value)
            if (v != isAnd)
                return !isAnd;

        return isAnd;
    }
}
