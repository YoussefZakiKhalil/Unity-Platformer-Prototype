using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTextRotator : MonoBehaviour
{
    Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam);
    }
}
