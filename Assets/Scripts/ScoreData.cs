using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



[Serializable]
public class ScoreData
{

    public int score;
    public float timeElapsed; //time in seconds
    public string name;


    public const float MAX_SCORE = 1000000f;

    public enum PrefStrings
    {
        MAX_SCORE,
        MIN_TIME_S,
        NAME
    }

    public ScoreData()
    {
        Reset();
    }


    public void Reset()
    {
        score = 0;
        timeElapsed = MAX_SCORE;
        name = "TBA";
    }

}