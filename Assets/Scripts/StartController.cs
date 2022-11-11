using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class StartController : MonoBehaviour, CUAS.MMT.IBallDesign
{

    public string firstScene = "Level_1";

    public TextMeshProUGUI infoText;

    public TextMeshProUGUI scores;

    public TextMeshProUGUI highscore;

    public GameObject loadScores;

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
        StatusController.Instance.ResetAll();
        //PlayerPrefs.DeleteKey("Highscore");
        UnityEngine.Debug.Log("active scene: " + SceneManager.GetActiveScene().name);
        highscore.text = "Highscore: " + PlayerPrefs.GetInt("Highscore").ToString();
        loadScores.GetComponent<SaveController>().LoadScores();



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

            Exit();

        }

    }


    void Exit()
    {

        UnityEngine.Debug.Log("Exit");
        Application.Quit();

    }


    // Update is called once per frame
    void Update()
    {


    }


    public void OnButtonStartPressed()
    {

        SceneManager.LoadScene(firstScene);

    }


    public void OnButtonExitPressed()
    {

        UnityEngine.Debug.Log("OnButtonExitPressed");

        Exit();

    }

}
