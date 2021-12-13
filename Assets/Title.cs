using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : GOISSceneManager
{
    [SerializeField]
    private float fadeSpan, titleTime;
    [SerializeField]
    private string nextScene, demoScene;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private UnityEngine.UI.Image fade;

    // Update is called once per frame
    void Update()
    {
    }

    public void OnStart()
    {
        Next(nextScene);
    }

    void SetAlpha(float t)
    {
        Color c = fade.color;

        c.a = t / fadeSpan;

        fade.color = c;
    }

    // Use this for initialization
    IEnumerator Start()
    {
        yield return FadeIn();

        startButton.SetActive(true);

        yield return new WaitForSeconds(titleTime);

        Next(demoScene);
    }

    IEnumerator FadeIn()
    {
        for (float t = fadeSpan; t > 0f; t -= Time.deltaTime)
        {
            SetAlpha(t);

            yield return null;
        }

        SetAlpha(0f);
    }
}
