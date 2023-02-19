using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarStone : MonoBehaviour
{
    [SerializeField]
    int axonThreshold = 1;

    [SerializeField]
    GameObject astralGate;

    Transform astralBody;

    int axonCounter = 0;

    bool isInteracting = false;

    public int AxonCounter
    {
        get { return axonCounter; }
        set 
        { 
            if(axonCounter < axonThreshold)
            {
                axonCounter = value; 
            }
        }
    }

    private void Update()
    {
        if(axonCounter >= axonThreshold)
        {
            isInteracting = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isInteracting)
        {
            astralGate.SetActive(false);
            collision.GetComponent<AstralBodyMovement>().ReturnToMainBody();
            axonCounter = 0;
            isInteracting = false;
        }
    }
}
