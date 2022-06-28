using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _health;
    public int health {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScreen");
    }
}
