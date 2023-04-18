using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradehandling : MonoBehaviour
{
    public GameObject astext;
    public GameObject datext;
    public GameObject mstext;
    public GameObject player;
    public GameObject aim2;
    public GameObject aim3;
    public GameObject upgradeui;
    public GameObject upgradepointstext;

    private int upgradepoints = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainUpgradePoint() {
        upgradepoints += 1;
        upgradepointstext.GetComponent<TMPro.TextMeshProUGUI>().text = "Upgrade Points: " + upgradepoints;
        upgradeui.SetActive(true);
    }

    private bool Upgradable() {
        if (upgradepoints > 0)
        {
            upgradepoints -= 1;
            upgradepointstext.GetComponent<TMPro.TextMeshProUGUI>().text = "Upgrade Points: " + upgradepoints;
            if (upgradepoints <= 0) {
                upgradeui.SetActive(false);
            }
            return true;
        }
        else {
            Debug.Log("Not enough points to upgrade");
            return false;
        }
        
    }

    public void UpgradeDamage(int amount) {

        if (Upgradable()){
            player.GetComponent<Weapon>().IncreaseDamage(amount * 50);
            Debug.Log("Successfully upgraded Damage");
            datext.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<Weapon>().currentdamage.ToString();
        }
        
    }


    public void UpgradeAttackSpeed(int amount)
    {
        if (Upgradable())
        {
            player.GetComponent<Weapon>().IncreaseAttackSpeed(amount);
            Debug.Log("Successfully upgraded AS");
            float help = (1f / player.GetComponent<Weapon>().firerate);
            astext.GetComponent<TMPro.TextMeshProUGUI>().text = help.ToString();
        }
    }
    public void UpgradeMovementSpeed(float amount)
    {
        if (Upgradable())
        {
            player.GetComponent<playermovement>().IncreaseMovementSpeed(amount);
            aim2.GetComponent<aim>().IncreaseMovementSpeed(amount);
            aim3.GetComponent<aim>().IncreaseMovementSpeed(amount);
            Debug.Log("Successfully upgraded MS");
            mstext.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<playermovement>().mSpeed.ToString();
        }
    }
}
