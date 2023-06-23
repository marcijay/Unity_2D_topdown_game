using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    public string startingLevelName;
    public string menuLevelName;

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(startingLevelName);
    }

    public void OnReurnButtonClick()
    {
        SceneManager.LoadScene(menuLevelName);
    }
}
