using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject player;
    public GameObject prefab;

    public float secondsBetweenSpawn = 1;

    public int maxSpawns = 10;
    private int spawns = 0;

    //public float speed = 10;
    //public float radius = 2f;

    float elapsedTime = 0; //time since last spawn

    // Start is called before the first frame update
    void Start()
    {

 
    }

    // Update is called once per frame
    void Update()
    {

        elapsedTime += Time.deltaTime;

        if ((spawns < maxSpawns) && (elapsedTime > secondsBetweenSpawn))
        {

            GameObject enemy = Instantiate<GameObject>(prefab);
            enemy.transform.position = transform.position;
            enemy.GetComponent<EnemyController>().targetTransform = player.transform;
            //enemy.GetComponent<EnemyController>().speed = speed;
            //enemy.GetComponent<EnemyController>().radius = radius;

            elapsedTime = 0;

            spawns++;

        }

    }
}
