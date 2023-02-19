using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralBodyMovement : MonoBehaviour
{
    /*For Altar Stone system
      OnTriggerEnter(AstralGate(?)), call AstralForm in PlayerMovement
      Then, in this script, if OnTriggerStay near an axon stone and pressing 'E', increase axon stone counter
      and destroy the object
      Then, if OnTriggerStay near an altar stone, and presssing 'E', copy the axonStoneCounter to the AltarStone Script
      and set this.axonCounter to 0
      Then, in the AltarStone Script, if axonStoneCounter is over a threshold, disable the AstralGateObject
    */

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f, maxAirSpeed = 10f, maxAcceleration = 10f, maxAirAcceleration = 10f;

    [SerializeField, Range(0f, 100f)]
    float jumpHeight = 1f;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField, Min(0)]
    float probeDistance = 0.55f;

    [SerializeField]
    GameObject mainBody;
    
    int axonCounter = 0;

    float speed;
    float acceleration;
    float desiredVelocity;

    float velocity;

    Rigidbody2D rb;

    bool OnGround => Physics2D.Raycast(transform.position, Vector2.down, probeDistance, groundMask);

    bool isInteracting = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
        Jump();

        if (Input.GetKeyDown(KeyCode.M))
        {
            ReturnToMainBody();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isInteracting = true;
        }
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        acceleration = OnGround ? maxAcceleration: maxAirAcceleration;
        speed = OnGround ? maxSpeed: maxAirSpeed;
        
        desiredVelocity = x * speed;

        float maxSpeedChange = acceleration * Time.deltaTime;

        rb.velocity += Mathf.MoveTowards(velocity, desiredVelocity, maxSpeedChange) * Vector2.right;
    }

    private void Jump()
    {
        if (OnGround && Input.GetButtonDown("Jump"))
        {
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            rb.velocity += jumpSpeed * Vector2.up;
        }
    }

    public void ReturnToMainBody()
    {
        mainBody.GetComponent<PlayerMovement>().enabled = true;
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isInteracting)
        {
            if (collision.CompareTag("Axon"))
            {
                axonCounter += 1;
                Destroy(collision.gameObject);
                isInteracting = false;
            }
            else if (collision.CompareTag("Altar"))
            {
                print("Hello There");
                collision.GetComponent<AltarStone>().AxonCounter += axonCounter;
                axonCounter = 0;
                isInteracting = false;
            }
        }
    }
}
