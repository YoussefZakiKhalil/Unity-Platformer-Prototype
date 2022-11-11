using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisonRotator : MonoBehaviour
{

    public GameObject enemy;

    private Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {

        offset = transform.position - enemy.transform.position;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        transform.position = enemy.transform.position+ offset;

    }
}
