using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimationFishEnemy : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator= gameObject.GetComponent<Animator>();
    }

    public void playDeathAnim()
    {
        //Debug.Log("EnemyDie");
        animator.Play("DeathAnim");
    }

}
