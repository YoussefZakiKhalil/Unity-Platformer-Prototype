using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public GameObject player;
    public GameObject healthBar;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnUpdateHealthBar(float value)
    {
        Debug.Log("OnUpdateHealthbar" + value);
        if(value >= 0 && value <= 1)
        {
            transform.GetChild(1).localScale = new Vector3(value, 1, 1);
        }
       
    }
}
