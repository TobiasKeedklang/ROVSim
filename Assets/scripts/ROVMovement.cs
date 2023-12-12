using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ROVMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float upForce = 10.0f;
    public float rotationSpeed = 1.0f;

    private Rigidbody rb;
    public Slider UpSlider;
    public TextMeshProUGUI UpText;

    private float horizontalInput;
    private float verticalInput;
    private float horizontalCamInput;
    private float verticalCamInput;

    private bool bIncrease;
    private bool bDecrease;
    private bool bRotReset;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bIncrease = false;
        bDecrease = false;
        bRotReset = false;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
       
        RotateObject();
        changeUpwardsForce();

        if (Input.GetKeyDown(KeyCode.T) && !bRotReset)
        {
            StartCoroutine(RotateToUpright());
        }

    }

    void FixedUpdate()
    {
        // Calculate the movement direction based on the object's rotation
        Vector3 movementDirection = transform.forward * verticalInput + transform.right * horizontalInput /*+ transform.up * lateralInput*/;
        movementDirection.Normalize();

        // Apply force to the Rigidbody for movement
        rb.AddForce(movementDirection * moveSpeed);
        rb.AddForce(Vector3.up * upForce);
    }

    void RotateObject()
    {
        if (!bRotReset) // Only rotate manually when not in the RotateToUpright process
        {
            horizontalCamInput = Input.GetAxis("HorisontalView");
            verticalCamInput = Input.GetAxis("VerticalView");

            if (Mathf.Abs(horizontalCamInput) > 0.1f || Mathf.Abs(verticalCamInput) > 0.1f)
            {
                Vector3 rotation = new Vector3(verticalCamInput * rotationSpeed, horizontalCamInput * rotationSpeed, 0f);
                transform.Rotate(rotation);
            }
        }
    }

    IEnumerator RotateToUpright()
    {
        bRotReset = true;

        Quaternion targetRotation = Quaternion.identity;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure the final rotation is exactly the target rotation
        bRotReset = false;
    }

    void changeUpwardsForce()
    {
        //Check for the increase in upwards force
        if (Input.GetKeyDown(KeyCode.UpArrow) && upForce < 15 && !bIncrease)
        {
            upForce = upForce += 5;
            bIncrease = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && upForce > 0 && !bDecrease)
        {
            upForce = upForce -= 5;
            bDecrease = true;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            bIncrease = false;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            bDecrease = false;
        }

        UpSlider.value = upForce;
        UpText.text = "Motor strength: " + upForce;
    }
}

