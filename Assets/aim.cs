using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class aim : MonoBehaviour
{
    public float mSpeed = 5f;
    public Camera cam;
    Vector2 movement;
    Vector2 mousePos;
    public Rigidbody2D rb;
    public Rigidbody2D playerrb;
    public SpriteRenderer sprite;
    public float xoffset;
    public float yoffset;
    //public float helpvertical;
    //public float helpspeed;

    // Update is called once per frame
    void Update()
    {

        //movement.x = UnityEngine.Input.GetAxisRaw("Horizontal");
        //movement.y = UnityEngine.Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        

    }

    public void IncreaseMovementSpeed(float amount)
    {
        this.mSpeed += amount;
    }

    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + movement.normalized * mSpeed * Time.fixedDeltaTime);
        Vector2 newposition = new Vector2(playerrb.position.x + xoffset, playerrb.position.y +yoffset);
        rb.MovePosition(newposition);
        Vector2 lookDir = mousePos - rb.position;
        
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg -90f;
        if ((angle < 0) && (angle > -180)) //between 0 and -180 mouse at the right
        {
            sprite.flipX = true;
        }
        else {
            sprite.flipX = false;
        }
        rb.rotation = angle;
    }

    public void ChosenFirepoint() {
        sprite.enabled = true;
    }

    public void NotChosenFirepoint() {
        sprite.enabled = false;
    }
}
