using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class EnemyController : MonoBehaviour
{

    // Start is called before the first frame update
    public float speed = 15; //attack speed
    public float radius = 7f; //attack radius

    public float damage = 0.0f;
    public bool invincible = false;

    public Transform targetTransform;
    public Rigidbody rb;

    void Start()
    {

        Debug.Log("EnemyController: Start");

        rb = GetComponent<Rigidbody>();

    }


    // Update is called once per frame
    void Update()
    {

        //check radius
        Vector3 dir = targetTransform.position - gameObject.transform.position;
        dir.y = 0;

        float d = dir.magnitude;
        //Debug.Log("distance: " + d);

        if (d <= radius)
        {
            rb.velocity += dir.normalized * speed * Time.deltaTime;
        }

    }


    //trigger functions
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.CompareTag("Lava"))
        {
            Destroy(gameObject);

        }

    }


}
