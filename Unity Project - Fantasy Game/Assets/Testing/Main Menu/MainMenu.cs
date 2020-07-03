using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject confirmExit;

    public GameObject helpMenu;

    SceneLoader loader;
    private void Start()
    {
        loader = FindObjectOfType<SceneLoader>();
    }
    public void Play()
    {
        loader.LoadScene(1);
    }
    public void ToggleHelp(bool active)
    {
        helpMenu.SetActive(active);
    }
    public void Exit()
    {
        confirmExit.SetActive(true);
    }
    public void ConfirmExit()
    {
        Application.Quit();
    }
    public void CancelExit()
    {
        confirmExit.SetActive(false);
    }
}
