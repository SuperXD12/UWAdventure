using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{

    public float mSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public float helpvertical;
    public float helpspeed;

    Vector2 movement;


    // Update is called once per frame
    void Update()
    {
        /*movement.x =UnityEngine.Input.GetAxisRaw("Horizontal");
        movement.y = UnityEngine.Input.GetAxisRaw("Vertical");
        helpvertical = movement.x;
        helpspeed = movement.sqrMagnitude;

        animator.SetFloat("Speed", helpspeed);
        animator.SetFloat("Vertical", helpvertical);*/





    }

    public void IncreaseMovementSpeed(float amount) {
        this.mSpeed += amount;
    }

    void FixedUpdate() {
        movement.x = UnityEngine.Input.GetAxisRaw("Horizontal");
        movement.y = UnityEngine.Input.GetAxisRaw("Vertical");
        helpvertical = movement.x;
        helpspeed = movement.sqrMagnitude;

        animator.SetFloat("Speed", helpspeed);
        animator.SetFloat("Vertical", helpvertical);


        rb.MovePosition(rb.position+ movement.normalized*mSpeed*Time.fixedDeltaTime);

        //Vector2 helpvector = movement * mSpeed * Time.fixedDeltaTime;
        //animator.SetFloat("Vertical", (helpvector.y));
        //Debug.Log(helpvector.y);
        //animator.SetFloat("Speed", (helpvector.x));
        //Debug.Log(helpvector.x);



    }

   
}
