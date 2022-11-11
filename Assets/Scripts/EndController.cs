using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class EndController : MonoBehaviour
{
    private Rigidbody rb;

    private Vector2 mvG = new Vector2();

    [SerializeField]
    private float speed = 10;

    public TextMeshProUGUI winText;

    public TextMeshProUGUI highscoreText;

    int highscore = 0;

    [SerializeField]
    BallDesign defaultDesign;


    public void ApplyBallDesign(BallDesign d, bool invincible = false)
    {


        GetComponent<MeshRenderer>().material = d.ball;


        for (int i = 0; i < transform.childCount; i++)
        {

            transform.GetChild(i).GetComponent<MeshRenderer>().material = d.rings;
        }


    }


    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();

        //UnityEngine.Debug.Log("active scene: " + SceneManager.GetActiveScene().name);

        UnityEngine.Debug.Log("last scene detected: " + StatusController.Instance.GetTimeTotalMS() + "ms");


        winText.text = "Victory! (" + (int)(StatusController.Instance.GetTimeTotalMS() / 1000) + "s, " + " Score: " + StatusController.Instance.GetScoreTotal() + ")";
        saveHighscore();

        //StartCoroutine(WaitAndLoadScene(0, secondsWait));

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

    }





    public void OnMove(InputAction.CallbackContext context)
    {
        //moving
        mvG = context.ReadValue<Vector2>();
    }


    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(mvG.x, 0, mvG.y) * speed);
    }

    void saveHighscore()
    {
        highscore = StatusController.Instance.GetScoreTotal();
        if (highscore > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", highscore);
            PlayerPrefs.Save();
            highscoreText.gameObject.SetActive(true);
        }
    }
}

