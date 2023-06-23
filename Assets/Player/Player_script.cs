using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player_script : MonoBehaviour
{
    [Range(1f, 10f)]
    public float characterSpeed;
    [Range(1f, 10f)]
    public float arrowSpeed;

    public GameObject Arrow;
    Input input;
    new Rigidbody2D rigidbody;
    Animator animator;
    AudioSource audioSource;
    Vector2 movementVector;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        input = new Input();
        input.PlayerInput.Movement.Enable();
        input.PlayerActions.Shoot.Enable();
        input.PlayerActions.Shoot.performed += Shoot_performed;
    }

    private void OnDestroy()
    {
        input.PlayerActions.Shoot.performed -= Shoot_performed;
        input.PlayerInput.Movement.Disable();
        input.PlayerActions.Shoot.Disable();
    }

    private void Shoot_performed(InputAction.CallbackContext obj)
    {
        input.PlayerActions.Shoot.Disable();
        animator.SetTrigger("Shoot");
    }

    public void Shoot()
    {
        Vector2 targetVecor = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - gameObject.transform.position).normalized;
        var angle = Mathf.Atan2(targetVecor.y, targetVecor.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        GameObject arrow = Instantiate(Arrow, gameObject.transform.position, rotation);
        arrow.GetComponent<Rigidbody2D>().AddForce(targetVecor * arrowSpeed, ForceMode2D.Impulse);
        audioSource.Play();
        input.PlayerActions.Shoot.Enable();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //rigidbody.MovePosition(-collision.relativeVelocity + rigidbody.position * characterSpeed * 2);
    }

    private void Update()
    {
        movementVector = input.PlayerInput.Movement.ReadValue<Vector2>();
        animator.SetInteger("Movement_vertical", Mathf.RoundToInt(movementVector.y));
        animator.SetInteger("Movement_horizontal", Mathf.RoundToInt(movementVector.x));
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + movementVector * characterSpeed * Time.fixedDeltaTime);
    }
}
