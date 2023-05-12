using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollowEnemy : MonoBehaviour
{
    
    //private Transform objectToFollow;
    //public GameObject gameobjectoFollow;
    public GameObject nameText;
    //private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        //rectTransform = gameobjectoFollow.GetComponent<RectTransform>();
        //objectToFollow = gameObject.GetComponent<Transform>();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        //if (objectToFollow != null)
           // rectTransform.anchoredPosition = objectToFollow.localPosition;
        
    }

    public void SetName(string name, Color color) { 
        nameText.GetComponent<TMPro.TextMeshPro>().text = name;
        nameText.GetComponent<TMPro.TextMeshPro>().color = color;
    }
}
