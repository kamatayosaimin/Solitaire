using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    [SerializeField]
    private float fadeSpan, clearStartSpan, buttonSpan, clearSpan;
    [SerializeField]
    private CanvasGroup group;
    [SerializeField]
    private GameObject buttons, particle;
    [SerializeField]
    private Transform particlePoints;
    [SerializeField]
    private UnityEngine.UI.Text clear;

    // Update is called once per frame
    void Update()
    {
    }

    void ApplyParticle()
    {
        foreach (Transform p in particlePoints)
            ApplyParticle(p);
    }

    void ApplyParticle(Transform point)
    {
        Transform t = Instantiate(particle, point).transform;

        t.eulerAngles = Vector3.left * 90f;
    }

    // Use this for initialization
    IEnumerator Start()
    {
        yield return FadeIn();

        yield return new WaitForSeconds(clearStartSpan);

        ApplyParticle();

        StartCoroutine(ManHunt());

        yield return new WaitForSeconds(buttonSpan);

        buttons.SetActive(true);
    }

    IEnumerator FadeIn()
    {
        for (float t = 0f; t < fadeSpan; t += Time.deltaTime)
        {
            group.alpha = t / fadeSpan;

            yield return null;
        }

        group.alpha = 1f;
    }

    IEnumerator ManHunt()
    {
        bool tenMetsu = false;

        clear.gameObject.SetActive(true);

        while (true)
        {
            tenMetsu = !tenMetsu;

            clear.color = tenMetsu ? Color.red : Color.black;

            yield return new WaitForSeconds(clearSpan);
        }
    }
}
