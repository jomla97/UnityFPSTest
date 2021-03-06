﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform camera;
    public CharacterController controller;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float mouseSensitivity = 150f;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float gravity = -20f;
    public float jumpHeight = 1.5f;

    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            //Camera movement
            if(!paused){
              float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
              float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

              xRotation -= mouseY;
              xRotation = Mathf.Clamp(xRotation, -90f, 90f);

              camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
              transform.Rotate(Vector3.up * mouseX);
            }

            //Walking & running
            float x = 0;
            float z = 0;
            float movementSpeed = walkSpeed;

            if(!paused){

              if (Input.GetKey(KeyCode.LeftShift))
              {
                  movementSpeed = runSpeed;
              }

              x = Input.GetAxis("Horizontal");
              z = Input.GetAxis("Vertical");
            }

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * movementSpeed * Time.deltaTime);

            //Jump
            if (Input.GetButtonDown("Jump") && isGrounded && !paused)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            //Gravity
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }

    public void Resume(){
      paused = false;
    }

    public void Pause(){
      paused = true;
    }
}
