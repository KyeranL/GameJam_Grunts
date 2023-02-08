using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System;
using Mono.Cecil.Cil;



public class Player_Move : MonoBehaviour
{
    #region Movement Variables
    //8 DIRECTION MOVEMENT
    float vertical;
    float horizontal;

    float verticalRaw;
    float horizontalRaw;

    Vector3 targetRotation;

    [Header("Player Status")]

    //Movement
    public float speed = 200f;
    public float rotationSpeed = 10;


    //Life//
    int Life;
    int Atk;
    #endregion

    #region Player Components

    Rigidbody rb;

    #endregion

    #region Habilities

    //Dash
    public bool isHolding = false;
    public float timetoPress;
    public float timetoPressBase;
    public int DashForce;
    public int[] keyPressCounts = new int[8];
    private float cooldownTimer;
    public float cooldownDuration = 1f;
    public KeyCode lastKey = KeyCode.None;
    public float resetTimer;
    public float resetDuration = 2f;
    public bool hasDash;


    // Jump

    public float jumpForce;
    public float turnOffDuration = 2.0f; // How long to turn off gravity for
    private float turnOffTime; // Time when gravity was turned off
    //Basic Atak

    //Shoot

    //Blast

    #endregion

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();

        


    }

    public void Update()
    {
        VerifyHold();

        CheckDash();

        Jump();

        GravityOn();

    }

    public void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        horizontalRaw = Input.GetAxisRaw("Horizontal");
        verticalRaw = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(horizontal, 0, vertical);
        Vector3 inputRaw = new Vector3(horizontalRaw, 0, verticalRaw);

        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        if (inputRaw.sqrMagnitude > 1f)
        {
            inputRaw.Normalize();
        }

        if (inputRaw != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input).eulerAngles;
        }

        rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation.x, Mathf.Round(targetRotation.y / 45) * 45, targetRotation.z), Time.deltaTime * rotationSpeed);
        //Movement

        Vector3 vel = input * speed;

        vel.y = rb.velocity.y - 0.5f;
        rb.velocity = vel;
      

    }

    public void CheckDash()
    {
        Dash(KeyCode.UpArrow, 0);
        Dash(KeyCode.DownArrow, 1);
        Dash(KeyCode.LeftArrow, 2);
        Dash(KeyCode.RightArrow, 3);
        Dash(KeyCode.W, 4);
        Dash(KeyCode.A, 5);
        Dash(KeyCode.S, 6);
        Dash(KeyCode.D, 7);

    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Apply the jump force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Turn off gravity
            rb.useGravity = false;
            turnOffTime = Time.time;
        }
    }

    public void GravityOn()
    {
        if (rb.useGravity && Time.time >= turnOffTime + turnOffDuration)
        {
            // Turn gravity back on
            rb.useGravity = true;
        }
    }
    #region DASH
    void Dash(KeyCode key, int index)
    {
        if (Input.GetKeyDown(key))
        {
           
            if (!isHolding)
            {

                if (key == lastKey)
                {
                    keyPressCounts[index]++;
                    if (keyPressCounts[index] == 2)
                    {
                        if (Time.time >= cooldownTimer)
                        {
                            rb.AddForce(transform.forward * DashForce, ForceMode.Impulse);
                            cooldownTimer = Time.time + cooldownDuration;
                        }
                        for (int i = 0; i < keyPressCounts.Length; i++)
                        {
                            keyPressCounts[i] = 0;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < keyPressCounts.Length; i++)
                    {
                        keyPressCounts[i] = 0;
                    }
                    keyPressCounts[index] = 1;
                }


            }
            else
            {
                if (key == lastKey)
                {

                    keyPressCounts[index]++;
                    if (keyPressCounts[index] == 3)
                    {
                        if (Time.time >= cooldownTimer)
                        {
                           
                            rb.AddForce(transform.forward * DashForce, ForceMode.Impulse);
                            cooldownTimer = Time.time + cooldownDuration;

                        }
                        for (int i = 0; i < keyPressCounts.Length; i++)
                        {
                            keyPressCounts[index] = 1; 
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < keyPressCounts.Length; i++)
                    {
                        keyPressCounts[i] = 0;
                    }
                    keyPressCounts[index] = 1;
                }

            }
            lastKey = key;
        }
        else
        {
            if (!isHolding)
            {
                if (Time.time > resetTimer)
                {
                    for (int i = 0; i < keyPressCounts.Length; i++)
                    {
                        keyPressCounts[i] = 0;
                    }
                    resetTimer = Time.time + resetDuration;
                }
            }
           
        }
    }

    #endregion

    #region VERIFY HOLD
    public void VerifyHold()
    {


        if (horizontal > 0 || vertical > 0 || horizontal < 0 || vertical < 0)
        {

            timetoPress -= Time.deltaTime;
            if (timetoPress < timetoPressBase / 2)
            {
                isHolding = true;
            }
            else
            {
                isHolding = false;

            }
        }
        else
        {
            isHolding = false;
            timetoPress = timetoPressBase;

        }
        if (timetoPress < 0)
        {
            timetoPress = 0;
        }
        

    }

    #endregion

   
}











