using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Arrow_srcipt : MonoBehaviour
{
    new Rigidbody2D rigidbody;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy_script>().OnHit();
        }
        Destroy(gameObject);
    }

    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyArrow());
    }
}
