using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROVMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float upForce = 15.0f;
    public float stability = 10.0f;
    public float rotationSpeed = 2.0f;
    private Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;
    bool bStable = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        RotateObjectWithMouse();
    }

    void FixedUpdate()
    {
        // Calculate the movement direction based on the object's rotation
        Vector3 movementDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        movementDirection.Normalize();

        // Apply force to the Rigidbody for movement
        rb.AddForce(movementDirection * moveSpeed);

        // Check for the space button input to jump
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * upForce);
        }

    }

    void RotateObjectWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 rotation = new Vector3(0f, mouseX * rotationSpeed, 0f);
        transform.Rotate(rotation);
    }

    void spawnDust()
    {

    }
}

