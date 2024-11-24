using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class ScientistController : MonoBehaviour
{

    private Vector2 velocity;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public bool isSelected;
    public bool isAlive = true;

    private float speed;
    private Vector2 targetDirection;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float smoothTime = 5f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isSelected)
        {
            HandleMovement();
        }

        SetAnimationState();
    }

    void SetAnimationState()
    {

        Debug.Log(speed);

        if (speed > 0.01f)
        {
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsIdle", false);
        } else
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsIdle", true);
        }

        if (targetDirection.x > 0.01f)
        {
            sprite.flipX = false;
        } else if (targetDirection.x < -0.01f)
        {
            sprite.flipX = true;
        }

    }


    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        targetDirection = new Vector2(horizontal, vertical);
        speed = targetDirection.magnitude;

        // Normalize the direction vector if it has non-zero magnitude
        if (targetDirection.sqrMagnitude > 0)
        {
            targetDirection.Normalize(); // Normalize the direction
        }
        // Calculate the target velocity with the run speed
        Vector2 targetVelocity;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            targetVelocity = targetDirection * runSpeed;
        } else
        {
            targetVelocity = targetDirection * walkSpeed;
        }

        // Smoothly dampen the velocity
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothTime);
    }
}
