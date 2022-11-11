using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using UnityEngine.Events;
using System;

public class PlayerControllernew : MonoBehaviour
{
    int count = 0;

    public TextMeshProUGUI countText;
    //public GameObject win;
    public UnityEvent onWin;
    public UnityEvent<float> onDamage;
    int scoreWin = 0;

    [SerializeField]
    private GameObject pickups;

    Stopwatch stopwatch = new Stopwatch();

    public int secondsWait = 3; //seconds for scene transition

    public float damage = 0.0f;
    float playerHealth = 1f;
    bool invincible = false;
    public float invincibleWaitSeconds = 3f;
    public string startScene = "Start";

    public GameObject wall;

    CapsuleCollider playerCollider;

    public UnityEvent onCollect; //event test

    //public Material invincibleMaterial;
    //public Material standardMaterial;

    private void Awake()
    {

        onCollect.AddListener(Collected);
    }
    void Collected()
    {

        UnityEngine.Debug.Log("Collected");

        SoundManager.Instance.PlaySound(SoundManager.Sound.Collect);

        count++;
        StatusController.Instance.AddScore(1);

        SetCountText();

    }

    // Start is called before the first frame update
    void Start()
    {
        scoreWin = pickups.transform.childCount;

        countText.text = "Score: 0";

        stopwatch.Reset();
        stopwatch.Start();

        playerCollider = GetComponent<CapsuleCollider>();

        SoundManager.Instance.Init();
    }

    public void OnExit(InputAction.CallbackContext context)
    {

        UnityEngine.Debug.Log("OnExit");

        if (context.performed)
        {

            UnityEngine.Debug.Log("Exit...");
            Application.Quit();

        }

    }

    // Update is called once per frame
    void Update()
    {
        onEnemyJump();
    }

    void SetCountText()
    {

        countText.text = "Score: " + count + "/" + scoreWin;

    }

    IEnumerator WaitAndLoadScene(int index, float delaySeconds)
    {
        //Print the time of when the function is first called.
        UnityEngine.Debug.Log("Started WaitAndLoadScene at timestamp : " + Time.time);

        yield return new WaitForSeconds(delaySeconds);

        UnityEngine.Debug.Log("Finished WaitAndLoadScene at timestamp : " + Time.time);

        SceneManager.LoadScene(index);

    }

    IEnumerator WaitDamageTaken(float delaySeconds)
    {
        //Print the time of when the function is first called.
        UnityEngine.Debug.Log("Started WaitAndLoadScene at timestamp : " + Time.time);

        yield return new WaitForSeconds(delaySeconds);

        UnityEngine.Debug.Log("Finished WaitAndLoadScene at timestamp : " + Time.time);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        //gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material = standardMaterial;
        //speed = 10;
        invincible = false;

    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.CompareTag("Pickup"))
        {

            collision.gameObject.SetActive(false);

            onCollect.Invoke();
        }

        if (collision.CompareTag("Goal"))
        {

            UnityEngine.Debug.Log("full score: " + StatusController.Instance.GetScoreTotal());
            stopwatch.Stop();

            //rb.isKinematic = true; //disable movement (fixxme!)

            UnityEngine.Debug.Log("time [ms]: " + stopwatch.ElapsedMilliseconds);

            StatusController.Instance.AddTimeMS(stopwatch.ElapsedMilliseconds);


            onWin.Invoke();
            //load next scene (timed)
            int index = SceneManager.GetActiveScene().buildIndex + 1;
            UnityEngine.Debug.Log("index: " + index);

            StartCoroutine(WaitAndLoadScene(index, secondsWait));

        }
        if (collision.CompareTag("Lava"))
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (collision.CompareTag("Object"))
        {
            wall.transform.Translate(16, 0, 0);
        }
        if ((collision.CompareTag("BossEnemy") ||collision.CompareTag("Enemy"))&& collision.gameObject.GetComponent<EnemyController>().damage > 0 && !invincible)
        {
            //speed = 5;
            playerHealth -= collision.gameObject.GetComponent<EnemyController>().damage;
            onDamage.Invoke(playerHealth);
            if (playerHealth <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            SoundManager.Instance.PlaySound(SoundManager.Sound.HitPlayer);
            invincible = true;
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            //gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material = invincibleMaterial;
            StartCoroutine(WaitDamageTaken(invincibleWaitSeconds));

        }
    }
    public void onEnemyJump()
    {

        RaycastHit hit;
        Ray landingRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(landingRay, out hit, playerCollider.bounds.size.y))
        {

            if (hit.collider.gameObject.tag == "Enemy")
            {
                SoundManager.Instance.PlaySound(SoundManager.Sound.HitEnemy);
                Destroy(hit.collider.gameObject);
            }

            if (hit.collider.gameObject.tag == "BossEnemy")
            {

                StatusController.Instance.playerJumpOnBoss = true;
            }

        }
    }

    public void OnButtonMenuPressed()
    {

        UnityEngine.Debug.Log("OnButtonMenuPressed");

        SceneManager.LoadScene(startScene);

    }
}
