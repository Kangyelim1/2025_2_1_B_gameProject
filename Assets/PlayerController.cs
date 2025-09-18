using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float rotationSpeed = 10.0f;

    [Header("점프 설정")]
    public float jumpHeight = 20f;
    public float gravity = -9.81f;
    public float Iandingduratiou = 0.3f;

    [Header("공격 설정")]
    public float attackDuration = 0.8f;
    public bool canMoveWhileAttacking = false;

    [Header("커포넌트")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    private float currentSpeed;
    private bool isAttacking = false;
    private bool isLanding = false;
    private float landingTimer;

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGroundded();

        HandleMovement();

        HandleLanding();

        UpdateAnimator();

        HandleAttack();

        HandleJump();
    }

    void HandleMovement()
    {
        if((isAttacking && !canMoveWhileAttacking) || isLanding)
        {
            currentSpeed = 0;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(horizontal != 0 || vertical != 0)
        {
           Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;

            if(Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0;
        }
    }

    void UpdateAnimator()
    {
        float animSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
        animator.SetFloat("Speed", animSpeed);

        animator.SetBool("isGrounded", isGrounded);

        bool isFalling = !isGrounded && velocity.y < -0.1;
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isLanding", isLanding);
    }

    void CheckGroundded()
    {
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        if (!isGrounded && wasGrounded)
        {
            Debug.Log("떨어지기 시작");
        }

        if ( isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;

            if(!wasGrounded && animator != null)
            {
                isLanding = true;
                landingTimer = Iandingduratiou;
            }
        }
    }
    
    void HandleJump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if(animator != null)
            {
                animator.SetTrigger("JumpTrigger");
            }
        }

        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }
    
    void HandleLanding()
    {
        if(isLanding)
        {
            landingTimer -= Time.deltaTime;
            if (landingTimer <= 0)
            {
                isLanding = false;
            }
        }

    }

    void HandleAttack()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackDuration;
            if (animator != null)
            {
                animator.SetTrigger("attackTrigger");
            }
        }
    }
}