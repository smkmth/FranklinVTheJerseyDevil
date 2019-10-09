using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator anim;

    public float walkSpeed;
    public float interactRadius;
    public bool isWalking;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        movement.Normalize();
        if (movement.x != 0 || movement.y != 0)
        {
            isWalking = true;
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
            isWalking = false;
        }
        transform.position += movement * walkSpeed * Time.deltaTime;

        if (Input.GetButtonDown("Interact"))
        {
            Collider2D[] overlapCircle = Physics2D.OverlapCircleAll(transform.position, interactRadius);
            GameObject currentClosest = gameObject;
            float currentClosestDist =999;
            foreach (Collider2D hit in overlapCircle)
            {
                if (hit.gameObject.layer == LayerMask.NameToLayer("NPC"))
                {
                    float dist = Vector2.Distance(hit.transform.position, transform.position);
                    if (dist < currentClosestDist)
                    {
                        currentClosestDist = dist;
                        currentClosest = hit.gameObject;

                    }
                }
            }
            if (currentClosest != gameObject)
            {
                Debug.Log(currentClosest.name);
            }

        }

    }


}
