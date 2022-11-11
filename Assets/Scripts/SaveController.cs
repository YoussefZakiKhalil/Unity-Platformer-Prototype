using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TMP_InputField name;
    public int secondsWait = 5; //seconds for scene transition
    const int NUMENTRIES = 5;
    public string fileName = "data.txt";
    static SerializableList<ScoreData> data = new SerializableList<ScoreData>();

    public void LoadScores()
    {
        data.dataList.Clear();
        if (!File.Exists(Application.persistentDataPath + "/" + fileName))
        {
            StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + fileName);
            for (int i = 0; i < NUMENTRIES; i++)
            {

                data.dataList.Add(new ScoreData());
            }
            sw.Write(JsonUtility.ToJson(data));
            sw.Close();
        }
        string json;
        StreamReader sr = null;
        using (sr = new StreamReader(Application.persistentDataPath + "/" + fileName))
        {
            json = sr.ReadToEnd();

        };

        JsonUtility.FromJsonOverwrite(json, data);
        GetScoreString();

    }

    public void SaveScores()
    {

        updateList();

        using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + fileName))
        {

            sw.Write(JsonUtility.ToJson(data));
            sw.Close();
        }
        StartCoroutine(WaitAndLoadScene(0, secondsWait));
    }

    public void updateList()
    {
        ScoreData newScore = new ScoreData() { score = StatusController.Instance.GetScoreTotal(), timeElapsed = StatusController.Instance.GetTimeTotalMS(), name = name.text };
        data.dataList.Add(newScore);
        Sort();
        data.dataList.Remove(data.dataList[5]);

    }

    private void Sort()
    {
        int size = NUMENTRIES + 1;
        for (int i = 1; i < size; i++)
        {
            for (int j = 0; j < (size - i); j++)
            {
                if (data.dataList[j].score < data.dataList[j + 1].score)
                {
                    ScoreData temp = data.dataList[j];
                    data.dataList[j] = data.dataList[j + 1];
                    data.dataList[j + 1] = temp;
                }
            }
        }
    }

    IEnumerator WaitAndLoadScene(int index, float delaySeconds)
    {
        //Print the time of when the function is first called.
        UnityEngine.Debug.Log("Started WaitAndLoadScene at timestamp : " + Time.time);

        yield return new WaitForSeconds(delaySeconds);

        UnityEngine.Debug.Log("Finished WaitAndLoadScene at timestamp : " + Time.time);

        SceneManager.LoadScene(index);
    }

    public void GetScoreString()
    {
        int place = 1;
        for (int index = 0; index < NUMENTRIES; index++)
        {
            float time = 1000000;
            if (data.dataList[index].timeElapsed != 1000000)
                time = data.dataList[index].timeElapsed / 1000;
            string substring = data.dataList[index].name.Substring(0, 3);
            scoreText.text = scoreText.text + "<mspace=16>" + place + "..." + substring.PadRight(6, ' ') + "| " + data.dataList[index].score.ToString().PadLeft(7, '0') + " | " + time.ToString() + "s\n";
            place++;
        }

    }

}
