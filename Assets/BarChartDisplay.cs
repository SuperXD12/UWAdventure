using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BarChartDisplay : MonoBehaviour
{

    public Sprite changeweapon_sprite;
    public Sprite bossvote_sprite;
    public Sprite buffplayer_sprite;
    public Sprite spawnmonster_sprite;
    public Sprite changemonster_sprite;
    public Sprite healplayer_sprite;

    public GameObject icon1;
    public GameObject icon2;
    public GameObject icon3;
    public GameObject icon4;
    public GameObject icon5;
    public GameObject icon6;

    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject text4;
    public GameObject text5;
    public GameObject text6;

    public GameObject bar1;
    public GameObject bar2;
    public GameObject bar3;
    public GameObject bar4;
    public GameObject bar5;
    public GameObject bar6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Sprite parseSprite(string name) { 
    
        switch (name)
        {
            case "changeweapon_votes":
                return changeweapon_sprite;
            case "bossvote_votes":
                return bossvote_sprite;
            case "buffplayer_votes":
                return buffplayer_sprite;
            case "spawnmonster_votes":
                return spawnmonster_sprite;
            case "changemonster_votes":
                return changemonster_sprite;
            default:
                return healplayer_sprite;

        }

    }

    private GameObject parseText(int x) {

        switch (x) {
            case 0:
                return text1;
            case 1:
                return text2;
            case 2:
                return text3;
            case 3:
                return text4;
            case 4:
                return text5;
            default:
                return text6;
        }
    }

    private GameObject parseImage(int x)
    {

        switch (x)
        {
            case 0:
                return icon1;
            case 1:
                return icon2;
            case 2:
                return icon3;
            case 3:
                return icon4;
            case 4:
                return icon5;
            default:
                return icon6;
        }
    }

    private GameObject parseBar(int x)
    {

        switch (x)
        {
            case 0:
                return bar1;
            case 1:
                return bar2;
            case 2:
                return bar3;
            case 3:
                return bar4;
            case 4:
                return bar5;
            default:
                return bar6;
        }
    }

    //0-400
    private void scaleBars(List<KeyValuePair<string, int>> myList) {
        float max = myList[0].Value;
        if (max > 0) {
            float onepercentm = max / 100;
            float onepercentb = 5f;

            for (int x = 0; x < 6; x++) {
                GameObject currbar = parseBar(x);
                float currvalue = myList[x].Value;
                
                float height = (currvalue / onepercentm) * onepercentb;
                //Debug.Log("Ive got value: " + currvalue + " with height: " + height);
                currbar.GetComponent<RectTransform>().sizeDelta = new Vector2(20, height);

            }

        }
    }


    public void UpdateChart(int changeweapon_votes, int bossvote_votes, int buffplayer_votes, int spawnmonster_votes, int changemonster_votes, int healplayer_votes) {
        List<KeyValuePair<string, int>> myList = new List<KeyValuePair<string, int>>();
        myList.Add(new KeyValuePair<string, int>("changeweapon_votes", changeweapon_votes));
        myList.Add(new KeyValuePair<string, int>("bossvote_votes", bossvote_votes));
        myList.Add(new KeyValuePair<string, int>("buffplayer_votes", buffplayer_votes));
        myList.Add(new KeyValuePair<string, int>("spawnmonster_votes", spawnmonster_votes));
        myList.Add(new KeyValuePair<string, int>("changemonster_votes", changemonster_votes));
        myList.Add(new KeyValuePair<string, int>("healplayer_votes", healplayer_votes));

        myList.Sort(
            delegate (KeyValuePair<string, int> pair1,
            KeyValuePair<string, int> pair2)
            {
                return pair2.Value.CompareTo(pair1.Value);
            }
        );

        for (int i = 0; i < 6; i++) {
            KeyValuePair<string, int> curr = myList[i];
            GameObject curricon = parseImage(i);
            GameObject currtext = parseText(i);
            Sprite currsprite = parseSprite(curr.Key);
            if (curricon is null)
                throw new System.ArgumentNullException("curricon");
            if (currsprite is null)
                throw new System.ArgumentNullException("currsprite");
            if (curricon.GetComponent<UnityEngine.UI.Image>() is null)
                throw new System.ArgumentNullException("ImageUI");
            curricon.GetComponent<UnityEngine.UI.Image>().sprite = currsprite;
            currtext.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
        }

        scaleBars(myList);
        
    }
}
