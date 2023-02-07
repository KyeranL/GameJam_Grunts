using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float dashTime = 0.5f;
    public float dashInterval = 0.5f;
    public float rotationSpeed = 10f;

    private float dashTimer;
    private float dashKeyPressTime;
    private bool canDash = true;
    private bool isDashing;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        if (horizontal != 0f || vertical != 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                if (canDash)
                {
                    dashKeyPressTime = Time.time;
                    canDash = false;
                }
                else if (Time.time - dashKeyPressTime <= dashInterval)
                {
                    isDashing = true;
                    dashTimer = dashTime;
                }
            }
        }
        else
        {
            canDash = true;
        }

        if (isDashing)
        {
            rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }
    }
}
