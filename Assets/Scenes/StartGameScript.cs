using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    public string levelName;
    public void OnStartButtonClick()
    {
        Destroy(GameObject.FindWithTag("Player"));
        Destroy(GameObject.FindWithTag("Canvas"));
        SceneManager.LoadScene(levelName);
    }
}
