using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_script : MonoBehaviour
{
    public string levelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            LoadLevel();
        }
    }

    public void LoadLevel()
    {
        GameObject Player = GameObject.FindWithTag("Player");
        DontDestroyOnLoad(Player);
        DontDestroyOnLoad(GameObject.FindWithTag("Canvas"));
        SceneManager.LoadScene(levelName);
    }
}
