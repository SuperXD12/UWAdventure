using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class aim : MonoBehaviour
{
    public Sprite machinegun;
    public Sprite icegun;
    public Sprite flamethrower;

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

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        //movement.x = UnityEngine.Input.GetAxisRaw("Horizontal");
        //movement.y = UnityEngine.Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        if ((angle <= 0) && (angle >= -180)) //between 0 and -180 mouse at the right
        {
            player.GetComponent<Weapon>().Currfirepointright();
            sprite.flipX = true;
        }
        else
        {
            player.GetComponent<Weapon>().Currfirepointleft();
            sprite.flipX = false;
        }

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
        /*float tempx = lookDir.x;
        float tempy = lookDir.y;
        lookDir.x =  tempy;
        lookDir.y = -1 * tempx;*/
        
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg -90f;
        
        rb.rotation = angle;
    }

    public void ChosenFirepoint(int spell) {
        
        sprite.enabled = true;
    }

    public void NotChosenFirepoint() {
        sprite.enabled = false;
    }
}
