using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScript : MonoBehaviour
{
    string scene;

    [Header("UI Components")]
    public Image LoadingImg;
    public TextMeshProUGUI progressText;

    void Start()
    {
        scene = "Level " + Random.Range(1, 6).ToString();
        StartCoroutine(AsyncLoad());
    }

    IEnumerator AsyncLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            LoadingImg.fillAmount = progress;
            progressText.text = string.Format("{0:0}%", progress * 100);
            yield return null;
        }
    }
}