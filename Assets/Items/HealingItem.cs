using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    [Range(1, 100)]
    public int healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Health health = collision.GetComponent<Health>();
            int points = health.health;
            if (points < 100)
            {
                health.health = ((points + healthValue) > 100) ? 100 : (points + healthValue);
                FindObjectOfType<Canvas>().GetComponent<UI_scipt>().SetHealthText(collision.gameObject.GetComponent<Health>().health);
                Destroy(gameObject);
            }
        }
    }
    //private void OnTriggerEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Health health = collision.gameObject.GetComponent<Health>();
    //        int points = health.health;
    //        if (points < 100)
    //        {
    //            health.health = ((points + healthValue) > 100) ? 100 : (points + healthValue);
    //            FindObjectOfType<Canvas>().GetComponent<UI_scipt>().SetHealthText(collision.gameObject.GetComponent<Health>().health);
    //            Destroy(gameObject);
    //        }
    //    }
    //}
}
