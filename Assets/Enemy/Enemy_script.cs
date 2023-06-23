using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_script : MonoBehaviour
{
    [Range(1f, 10f)]
    public float speed;

    Vector2 desiredPosition;
    new Rigidbody2D rigidbody2D;
    Animator animator;

    IEnumerator standardBehaviour;
    IEnumerator aggressiveBehaviour;
    IEnumerator attackBehaviour;

    public event System.EventHandler OnDestroy;

    IEnumerator AttackHealth(Health health)
    {
        while (true)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            health.health -= 5;
            canvas.GetComponent<UI_scipt>().SetHealthText(health.health);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator WalkTowardsPlayer(GameObject player)
    {
        while (true)
        {
            desiredPosition = player.transform.position;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Walking()
    {
        while (true)
        {
            Vector2 position = transform.position;
            Vector2 newPosition = new Vector2(position.x + Random.Range(-1f, 1f), position.y + Random.Range(-1f, 1f));
            desiredPosition = newPosition;
            yield return new WaitForSeconds(Random.Range(0.2f, 1.5f));
        }
    }

    public void OnHit()
    {
        OnDestroy?.Invoke(this, null);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (standardBehaviour != null) StopCoroutine(standardBehaviour);
            aggressiveBehaviour = WalkTowardsPlayer(collision.gameObject);
            StartCoroutine(aggressiveBehaviour);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (aggressiveBehaviour != null) StopCoroutine(aggressiveBehaviour);
            standardBehaviour = Walking();
            StartCoroutine(standardBehaviour);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            attackBehaviour = AttackHealth(collision.gameObject.GetComponent<Health>());
            StartCoroutine(attackBehaviour);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (aggressiveBehaviour != null) StopCoroutine(attackBehaviour);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        desiredPosition = transform.position;
        standardBehaviour = Walking();
        StartCoroutine(standardBehaviour);
    }

    private void FixedUpdate()
    {
        Vector2 towards = Vector2.MoveTowards(gameObject.transform.position, desiredPosition, speed * Time.fixedDeltaTime);
        Vector2 direction = (Vector3)towards - gameObject.transform.position;
        animator.SetInteger("Movement_horizontal", Mathf.RoundToInt(direction.normalized.x));
        animator.SetInteger("Movement_vertical", Mathf.RoundToInt(direction.normalized.y));
        rigidbody2D.MovePosition(towards);
    }
}
