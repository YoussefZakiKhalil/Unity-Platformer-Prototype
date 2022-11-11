using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{



    public GameObject game;


    public void OnEnable()
    {
        game.SetActive(false);
    }


    public void OnDisable()
    {
        game.SetActive(true);        
    }

    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log("MenuController: start");


    }



}
