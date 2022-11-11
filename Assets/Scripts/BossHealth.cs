using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject boss;
    public GameObject healthBar;

    // Update is called once per frame
    public void OnUpdateHealthBar(float value)
    {
       
        if (value >= 0 && value <= 1)
        {
            transform.GetChild(1).localScale = new Vector3(value, 1, 1);
        }

    }
}
