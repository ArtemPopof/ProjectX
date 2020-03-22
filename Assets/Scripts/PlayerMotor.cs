using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {

    // Movement
    private const int LANE_DISTANCE = 2;
    private const float TURN_SPEED = 0.05f;
    private CharacterController controller;
    private float jumpForce = 8.0f;
    private float gravity = 16.0f;
    private float verticalVelocity;
    private int desiredLane = 1; // 0 = Left, 1 = Middle, 2 = Right

    private float startSpeed = 14.0f;
    private float speed;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIncreaseAmount = 0.1f;

    // Animation
    private Animator animator;

    private void Start() {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        speed = startSpeed;
    }

    private void Update() {
        if (!GameManager.Instance.IsRunning) {
            return;
        }

        if (Time.time - speedIncreaseLastTick > speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            speed += speedIncreaseAmount;

            GameManager.Instance.Properties.setProperty("multiplier", 1.0f + (speed - startSpeed));
        }

        // Gather the inputs on which lane we should be
        if (MobileInput.Instance.SwipeLeft) {
             MoveLane(false);
         }
         if (MobileInput.Instance.SwipeRight) {
             MoveLane(true);
         }

         // Calculate where we should be in the future
         Vector3 targetPosition = transform.position.z * Vector3.forward;
         if (desiredLane == 0) {
             targetPosition += Vector3.left * LANE_DISTANCE;
         } else if (desiredLane == 2) {
             targetPosition += Vector3.right * LANE_DISTANCE;
         }

         // Let's caclulate our move delta
         Vector3 moveVector = Vector3.zero;
         moveVector.x = (targetPosition - transform.position).normalized.x * speed;

         bool isGrounded = IsGrounded();
         // Calculate Y
         if (isGrounded) {
             verticalVelocity -= 0.1f;

             animator.SetBool("Grounded", isGrounded);

             if (MobileInput.Instance.SwipeUp) {
                 // Jump
                 animator.SetTrigger("Jump");
                 verticalVelocity = jumpForce;
             } else if (MobileInput.Instance.SwipeDown)
             {
                 // Slide
                 StartSliding();
                 Invoke("StopSliding", 1.0f);
             }
         } else {
             verticalVelocity -= (gravity * Time.deltaTime);

             // Fast Falling mechanic
             if (MobileInput.Instance.SwipeDown) {
                verticalVelocity = -jumpForce;
             }
         }

         moveVector.y = verticalVelocity;
         moveVector.z = speed;

         // Move the player
         controller.Move(moveVector * Time.deltaTime);

         // Rotate the player to where he is going
         Vector3 direction = controller.velocity;
         direction.y = 0;
         transform.forward = Vector3.Lerp(transform.forward, direction, TURN_SPEED);
    }

    private void MoveLane(bool goingRight) {
        desiredLane = goingRight ? ++desiredLane : --desiredLane;

        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool IsGrounded() {
        Ray groundRay = new Ray(
            new Vector3(
                controller.bounds.center.x,
                controller.bounds.center.y - controller.bounds.extents.y + 0.2f,
                controller.bounds.center.z), Vector3.down);

        return Physics.Raycast(groundRay, 0.2f + 0.1f);
    }

    public void StartRunning() {
        animator.SetTrigger("StartRunning");
    }

    private void StartSliding() {
        animator.SetBool("IsSliding", true);
        controller.radius /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
    }

    private void StopSliding() {
        animator.SetBool("IsSliding", false);
        controller.radius *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
                break;
        }
    }

    private void Crash()
    {
        animator.SetTrigger("Death");
        GameManager.Instance.OnPlayerDeath();
    }
}