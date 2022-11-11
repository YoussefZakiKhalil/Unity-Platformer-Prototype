using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.AI;

public class EnemyControllerCustom : EnemyController
{

    //public float damage = 0.1f; //damage to the player

    public float patrolSpeed = 10;
   

    public float radiusStart = 15; //chase radius

    public float chaseAttackSpeed = 15; //chase/attack speed

    public float radiusWaypoint = 5f; //waypoint distance for proceeding
    public Transform[] points;

    private int destPoint = 0; //index of the destination point

    public enum States { Patrol, Chase, Attack };

    States currentState = States.Patrol;

    public Material materialPatrol;
    public Material materialChase;
    public Material materialAttack;

    void Start()
    {

        Debug.Log("EnemyControllerCustom: Start");

        rb = GetComponent<Rigidbody>();

        invincible = false;

        GotoNextPoint();

    }


    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case States.Patrol:
            {

                //check conditions for transitions
                //-> player within r_start -> go to chasing
                Vector3 dir_player = targetTransform.position - gameObject.transform.position;
                dir_player.y = 0;

                float d_player = dir_player.magnitude;

                if(d_player < radiusStart)
                {
                    //go chasing
                    Debug.Log("within chasing range -> starting to chase");
                    SoundManager.Instance.PlaySound(SoundManager.Sound.Alert);
                    GetComponent<MeshRenderer>().material = materialChase;
                    currentState = States.Chase;
                    
                    break;
                }

                //handle state update
                //->check radius
                Vector3 dir_waypoint = points[destPoint].position - gameObject.transform.position; //check direction of dest. and player
                dir_waypoint.y = 0;

                float d_waypoint = dir_waypoint.magnitude;
                //Debug.Log("d_wp: " + d_waypoint);
                if (d_waypoint <= radiusWaypoint)
                {
                    GotoNextPoint();

                }
                else
                {

                    rb.velocity += dir_waypoint.normalized * patrolSpeed * Time.deltaTime;

                }

                break;

            }
            case States.Chase:
            {

                    //check conditions for transitions
                    //-> player outside of rstart -> go to patrol
                    Vector3 dir_player = targetTransform.position - gameObject.transform.position;
                    dir_player.y = 0;

                    float d_player = dir_player.magnitude;

                    if (d_player >= radiusStart)
                    {
                        //go chasing
                        Debug.Log("outside of chase range -> going back to patrolling");
                        SoundManager.Instance.PlaySound(SoundManager.Sound.Miss);
                        GetComponent<MeshRenderer>().material = materialPatrol;
                        currentState = States.Patrol;

                        break;
                    }
                    else if(d_player < radius)
                    {

                        Debug.Log("within attack range -> starting attack");
                        SoundManager.Instance.PlaySound(SoundManager.Sound.Alert);
                        GetComponent<MeshRenderer>().material = materialAttack;
                        currentState = States.Attack;

                        break;
                    }

                    //chase
                    rb.velocity += dir_player.normalized * chaseAttackSpeed * Time.deltaTime;

                    break;

            }
            case States.Attack:
            {
                //check conditions for transitions
                //-> player outside of attack radius -> go to chasing
         
                Vector3 dir_player = targetTransform.position - gameObject.transform.position;
                dir_player.y = 0;

                float d_player = dir_player.magnitude;

                if (d_player >= radius)
                {
                    Debug.Log("outside of attack range -> going back to chasing");
                    SoundManager.Instance.PlaySound(SoundManager.Sound.Miss);
                    GetComponent<MeshRenderer>().material = materialChase;
                    currentState = States.Chase;
                    break;
                }

                //chase
                rb.velocity += dir_player.normalized * chaseAttackSpeed * Time.deltaTime;

                break;

            }
            default:
            {

                break;

            }

        }

    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
        //Debug.Log("next index: " + destPoint);
    }


    void OnDrawGizmos()
    {
        //Debug.Log("OnDrawGizmos");

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(points[destPoint].position, 0.5f);

        Gizmos.color = Color.blue; //blue, yellow, red

        if(currentState == States.Patrol)
        {

            Gizmos.color = Color.blue;

        }
        else if (currentState == States.Chase)
        {
            Gizmos.color = Color.yellow;

        }
        else {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(transform.position, radiusStart);

    }




}
