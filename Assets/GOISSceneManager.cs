using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GOISSceneManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    protected void Next(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
