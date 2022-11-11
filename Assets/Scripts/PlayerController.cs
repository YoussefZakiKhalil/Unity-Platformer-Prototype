using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using UnityEngine.Events;
using System;


public class PlayerController : MonoBehaviour, CUAS.MMT.IBallDesign
{
    private Rigidbody rb;

    private Vector2 mvG = new Vector2();

    [SerializeField]
    public float speed = 10;

    [SerializeField]
    private float jumpForce = 5f;

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

    [SerializeField]
    BallDesign defaultDesign;

    public GameObject wall;


    SphereCollider playerCollider;

    public UnityEvent onCollect; //event test


    //-------------------------------------------------------------------
    //event test

    //public static event Action<int> myStaticEvent;

    //public delegate void ExampleDelegate(int a); //delegate definition
    //ExampleDelegate exampleDelegate; //delegate instance
    public void ApplyBallDesign(BallDesign d, bool invincible = false)
    {


        GetComponent<MeshRenderer>().material = d.ball;


        for (int i = 0; i < transform.childCount; i++)
        {

            transform.GetChild(i).GetComponent<MeshRenderer>().material = d.rings;
        }


    }


    private void Awake()
    {

        onCollect.AddListener(Collected);

        //myStaticEvent += MyFunction;

        //exampleDelegate += MyFunction;


    }



    void MyFunction(int number)
    {
        UnityEngine.Debug.Log("MyFunction: " + number);

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

        rb = GetComponent<Rigidbody>();

        countText.text = "Score: 0";

        //win.gameObject.SetActive(false);

        UnityEngine.Debug.Log("scoreWin: " + scoreWin);

        stopwatch.Reset();
        stopwatch.Start();

        playerCollider = GetComponent<SphereCollider>();

        SoundManager.Instance.Init();

        //exampleDelegate.Invoke(4);

        //myStaticEvent.Invoke(5);
        //ApplyBallDesign(ballDesign);


        if (StatusController.Instance.GetBallDesign().ball == null)
        {

            ApplyBallDesign(defaultDesign);

        }
        else
        {

            ApplyBallDesign(StatusController.Instance.GetBallDesign());
        }

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

        speed = 10;
        invincible = false;

    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.CompareTag("Pickup"))
        {

            collision.gameObject.SetActive(false);

            onCollect.Invoke();

            //myStaticEvent.Invoke(count);
            //exampleDelegate?.Invoke(0);
        }

        if (collision.CompareTag("Goal"))
        {

            UnityEngine.Debug.Log("full score: " + StatusController.Instance.GetScoreTotal());
            stopwatch.Stop();

            rb.isKinematic = true; 

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
        if (collision.CompareTag("BossEnemy") && collision.gameObject.GetComponent<EnemyController>().damage > 0 && !invincible)
        {
            speed = 5;
            playerHealth -= collision.gameObject.GetComponent<EnemyController>().damage;
            onDamage.Invoke(playerHealth);
            if (playerHealth <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            SoundManager.Instance.PlaySound(SoundManager.Sound.HitPlayer);
            invincible = true;
            StartCoroutine(WaitDamageTaken(invincibleWaitSeconds));

        }
      
    }


    public void OnMove(InputAction.CallbackContext context)
    {

        //moving
        mvG = context.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {
       
        RaycastHit hit;
        Ray landingRay = new Ray(transform.position, Vector3.down);
        // if (!context.performed)

        if (Physics.Raycast(landingRay, out hit, playerCollider.bounds.size.y))
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.Jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

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

    private void FixedUpdate()
    {

        rb.AddForce(new Vector3(mvG.x, 0, mvG.y) * speed);

    }


    public void OnButtonMenuPressed()
    {

        UnityEngine.Debug.Log("OnButtonMenuPressed");

        SceneManager.LoadScene(startScene);

    }

}
