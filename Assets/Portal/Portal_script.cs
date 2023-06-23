using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_script : MonoBehaviour
{
    public int leveToLoadNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            LoadLevel();
        }
    }

    public void LoadLevel()
    {
        if(leveToLoadNumber == 2)
        {
            DontDestroyOnLoad(GameObject.FindWithTag("Canvas"));
            DontDestroyOnLoad(GameObject.FindWithTag("Player"));
        }
        if(leveToLoadNumber == 0) 
        {
            Destroy(GameObject.FindWithTag("Player"));
            Destroy(GameObject.FindWithTag("Canvas"));
            SceneManager.LoadScene("EndScreen");
        }

        if(leveToLoadNumber != 0) SceneManager.LoadScene("Level_" + leveToLoadNumber);
    }
}
