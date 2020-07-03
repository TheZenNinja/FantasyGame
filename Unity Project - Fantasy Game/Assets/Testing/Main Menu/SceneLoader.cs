using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider bar;
    public TextMeshProUGUI text;

    public void LoadScene(int index)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsync(index));
    }
    IEnumerator LoadAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(progress);
            bar.value = progress;
            text.text = (progress * 100).ToString("N0");

            yield return null;
        }
        loadingScreen.SetActive(false);
    }
}
