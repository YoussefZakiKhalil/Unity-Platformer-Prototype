using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

//source: http://wiki.unity3d.com/index.php/Singleton
[Serializable]
public class StatusController : Singleton<StatusController>
{

    public enum PrefStrings
    {
        MAX_SCORE
    }


    public bool playerJumpOnBoss = false;
    float timeTotalMS = 0; //total time taken in milliseconds
    int score = 0;
    public bool teleported = false;

    BallDesign design = null;

    // (Optional) Prevent non-singleton constructor use.
    protected StatusController()
    {

    }

    public void SetBallDesign(BallDesign b)
    {

        design = b;

    }

    public BallDesign GetBallDesign()
    {

        if (design == null)
        {
            Debug.Log("Creating ball design");
            design = ScriptableObject.CreateInstance<BallDesign>();
        }
        else
        {
            Debug.Log("Getting ball design");

        }


        return design;

    }

    public void ResetAll()
    {
        timeTotalMS = 0;
        score = 0;
    }


    public void AddTimeMS(float timeMS)
    {

        timeTotalMS += timeMS;

    }


    public float GetTimeTotalMS() // total time
    {

        return timeTotalMS;

    }


    public void AddScore(int value) //add to current score
    {

        score += value;

    }


    public int GetScoreTotal() //total current score
    {

        return score;

    }



}