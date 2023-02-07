using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System;


[RequireComponent(typeof(Rigidbody))]
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
    public KeyCode currentKey = KeyCode.None;
    // Jump

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
        VerifyDoubleClick();

        CheckKey();

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

        Vector3 vel = input * speed * Time.deltaTime;
        rb.velocity = vel;

    }

    public void CheckKey()
    {
        CheckDoublePress(KeyCode.UpArrow, 0);
        CheckDoublePress(KeyCode.DownArrow, 1);
        CheckDoublePress(KeyCode.LeftArrow, 2);
        CheckDoublePress(KeyCode.RightArrow, 3);
        CheckDoublePress(KeyCode.W, 4);
        CheckDoublePress(KeyCode.A, 5);
        CheckDoublePress(KeyCode.S, 6);
        CheckDoublePress(KeyCode.D, 7);

        

    }


   void CheckDoublePress(KeyCode key, int index)
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
                            Debug.Log(key.ToString() + " pressed twice!");
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
                            Debug.Log(key.ToString() + " pressed twice!");
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
            lastKey = key;
        }
}

    void CheckKeyPress(KeyCode key, int index)
    {
        if (Input.GetKeyDown(key))
        {
            if (key == lastKey)
            {
                keyPressCounts[index]++;
            }
            else
            {
                for (int i = 0; i < keyPressCounts.Length; i++)
                {
                    keyPressCounts[i] = 0;
                }
                keyPressCounts[index] = 1;
            }
            lastKey = key;
        }


    }

    public void VerifyDoubleClick()
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

   
}


   








