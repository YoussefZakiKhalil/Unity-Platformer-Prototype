using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRocks : MonoBehaviour
{

    public GameObject prefab;
    public GameObject rocks;
    public GameObject player;

    public float secondsBetweenSpawn = 1;

    public int maxSpawns = 10;
    private int spawns = 0;
    float radius = 20f;

    public bool trigger = false;

    float elapsedTime = 0; //time since last spawn

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = player.transform.position - gameObject.transform.position;
        dir.y = 0;

        float d = dir.magnitude;
        if (d <= radius)
        {
            trigger = true;
        }
            elapsedTime += Time.deltaTime;

        if ((spawns < maxSpawns) && (elapsedTime > secondsBetweenSpawn) && trigger == true)
        {

            GameObject rock = Instantiate<GameObject>(prefab);
            rock.transform.SetParent(rocks.transform);
            rock.transform.localPosition = transform.localPosition;
            elapsedTime = 0;

            spawns++;

        }

    }
}
