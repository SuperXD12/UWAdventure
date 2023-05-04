using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_CommandList : MonoBehaviour
{
    private GameObject player;
    private GameObject gamelogic;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gamelogic = GameObject.FindGameObjectWithTag("GameLogic");
    }

    void Action_ChangeWeapon() {
        player.GetComponent<Weapon>().ExecuteAction_ChangeWeapon();
    }

    void Action_IncreasePlayerDamage(int viewercount) {
        StartCoroutine(TimedBuffDamage(viewercount));
    }
    private IEnumerator TimedBuffDamage(int viewercount) {
        int damagebuff = Mathf.RoundToInt(50 * Mathf.Min(1, 10 / viewercount));
        player.GetComponent<Weapon>().IncreaseDamage(damagebuff);
        float time = 10f * Mathf.Min(1, 10 / viewercount);
        Debug.Log("Temporarily Increased the Damage of the Player by " + damagebuff + " for "+time+" seconds");
        yield return new WaitForSeconds(time);
        player.GetComponent<Weapon>().IncreaseDamage(damagebuff*-1);
        Debug.Log("Temp. buffed Damage of the Player by " + damagebuff + " expired");
    }

    void Action_VoteSpawnBoss() { 
    
    }

}
