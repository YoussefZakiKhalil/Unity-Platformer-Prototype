using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayWenderlich.Unity.StatePatternInUnity;
using UnityEngine.Events;

public class EnemyControllerCustomSP : EnemyController
{
    public float patrolSpeed = 10;


    public float radiusStart = 15; //chase radius

    public float chaseAttackSpeed = 15; //chase/attack speed

    public GameObject goal;
    public GameObject bossHealthBar;
    public UnityEvent<float> onDamage;
    public float bossEnemyHealth = 1f;
    public float invincibleWaitSeconds = 2f;
    public float radiusWaypoint = 5f; //waypoint distance for proceeding
    public Transform[] points;
    public bool escapeHappened = false;

    public Material materialPatrol;
    public Material materialChase;
    public Material materialAttack;
    public Material materialEscape;
    public Material materialPhase2;

    public StateMachine enemySM = new StateMachine();
    public StatePatrol statePatrol;
    public StateChase stateChase;
    public StateAttack stateAttack;
    public StateEscape stateEscape;
    public StatePhase2 statePhase2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        invincible = false;

        statePatrol = new StatePatrol(this, enemySM);
        stateChase = new StateChase(this, enemySM);
        stateAttack = new StateAttack(this, enemySM);
        stateEscape = new StateEscape(this, enemySM);
        statePhase2 = new StatePhase2(this, enemySM);
        enemySM.Initialize(statePatrol);
    }

    // Update is called once per frame
    void Update()
    {
        enemySM.CurrentState.LogicUpdate();
    }
    IEnumerator WaitDamageTaken(float delaySeconds)
    {
        //Print the time of when the function is first called.
        UnityEngine.Debug.Log("Started WaitAndLoadScene at timestamp : " + Time.time);

        yield return new WaitForSeconds(delaySeconds);

        UnityEngine.Debug.Log("Finished WaitAndLoadScene at timestamp : " + Time.time);

        invincible = false;

    }
    private void OnTriggerEnter(Collider collision)
    {



        if (collision.CompareTag("Player") && collision.gameObject.GetComponent<PlayerControllernew>().damage > 0 && !invincible && StatusController.Instance.playerJumpOnBoss)
        {
            StatusController.Instance.playerJumpOnBoss = false;
            Debug.Log("EnemyHealth is:" + bossEnemyHealth + "damagelble" + invincible + " booljump = " + StatusController.Instance.playerJumpOnBoss);
            SoundManager.Instance.PlaySound(SoundManager.Sound.HitEnemy);
            bossEnemyHealth -= collision.gameObject.GetComponent<PlayerControllernew>().damage;
            onDamage.Invoke(bossEnemyHealth);
            

            if (bossEnemyHealth <= 0.1 || collision.CompareTag("Lava"))
            {
                Destroy(gameObject);
                goal.SetActive(true);
                bossHealthBar.SetActive(false);
            } 
            invincible = true;
            StartCoroutine(WaitDamageTaken(invincibleWaitSeconds));
           
        }
        if (collision.CompareTag("BallPlayer") && collision.gameObject.GetComponent<PlayerController>().damage > 0 && !invincible && StatusController.Instance.playerJumpOnBoss)
        {
            StatusController.Instance.playerJumpOnBoss = false;
            Debug.Log("EnemyHealth is:" + bossEnemyHealth + "damagelble" + invincible + " booljump = " + StatusController.Instance.playerJumpOnBoss);
            SoundManager.Instance.PlaySound(SoundManager.Sound.HitEnemy);
            bossEnemyHealth -= collision.gameObject.GetComponent<PlayerController>().damage;
            onDamage.Invoke(bossEnemyHealth);


            if (bossEnemyHealth <= 0.1 || collision.CompareTag("Lava"))
            {
                Destroy(gameObject);
                goal.SetActive(true);
                bossHealthBar.SetActive(false);
            }
            invincible = true;
            StartCoroutine(WaitDamageTaken(invincibleWaitSeconds));

        }
    }
    //void OnDrawGizmos()
    //{
    //    //Debug.Log("OnDrawGizmos");

    //    Gizmos.color = Color.white;
    //    Gizmos.DrawSphere(points[statePatrol.destPoint].position, 0.5f);

    //    Gizmos.color = Color.blue; //blue, yellow, red

    //    if (enemySM.CurrentState == statePatrol)
    //    {

    //        Gizmos.color = Color.blue;

    //    }
    //    else if (enemySM.CurrentState == stateChase)
    //    {
    //        Gizmos.color = Color.yellow;

    //    }
    //    else
    //    {
    //        Gizmos.color = Color.red;
    //    }

    //    Gizmos.DrawWireSphere(transform.position, radiusStart);

    //}
}
