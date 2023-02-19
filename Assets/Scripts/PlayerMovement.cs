using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    float groundSpeed = 10f, airSpeed = 2f;

    [SerializeField, Range(0f, 100f)]
    float jumpHeight = 1f, jumpAcceleration = 0.5f;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField, Min(0)]
    float probeDistance = 0.55f;

    [SerializeField]
    GameObject astralBody;

    float x, xDisp;

    float speed = 10f;

    Rigidbody2D rb;


    bool OnGround => Physics2D.Raycast(transform.position, Vector2.down, probeDistance, groundMask);

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
        Jump();

        if (Input.GetKeyDown(KeyCode.N))
        {
            AstralForm();
        }
    }

    private void Movement()
    {
        speed = OnGround ? groundSpeed : airSpeed;

        x = Input.GetAxis("Horizontal");
        xDisp = x * speed * Time.deltaTime;
        transform.position += new Vector3(xDisp, 0f, 0f);
    }
    
    private void Jump()
    {
        if (OnGround && Input.GetButtonDown("Jump"))
        {
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            rb.velocity += jumpSpeed * Vector2.up;
        }
    }

    private void AstralForm()
    {
        astralBody.SetActive(true);
        astralBody.transform.position = transform.position;
        GetComponent<PlayerMovement>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AstralGate"))
        {
            AstralForm();
        }
    }
}
