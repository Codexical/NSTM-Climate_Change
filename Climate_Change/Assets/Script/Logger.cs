using UnityEngine;
using System.IO;

public class Logger : MonoBehaviour
{
    private string filePath = @"./Log.csv";
    private string nowTime;
    private void Start()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Timestamp,UserAnswer,,,,,,,,,,,,,,,,,,,,UserStatus,,,,,,,,,,,,,,,,,,,,UseTime,IsPass\n");
        }
    }

    public void SetTime()
    {
        nowTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }

    public void LogData(int[] answers, int[] groundTruth, bool isPass)
    {
        string taskString = "";
        string answerString = "";
        for (int i = 0; i < answers.Length; i++)
        {
            taskString += answers[i].ToString() + ",";
        }
        for (int i = 0; i < answers.Length; i++)
        {
            if (answers[i] > 0)
            {
                if (answers[i] == groundTruth[i])
                {
                    answerString += "1,";
                }
                else
                {
                    answerString += "2,";
                }
            }
            else
            {
                answerString += "0,";
            }
        }
        int useTime = (int)(System.DateTime.Now - System.DateTime.Parse(nowTime)).TotalSeconds;
        string logEntry = $"{nowTime},{taskString}{answerString}{useTime},{isPass}\n";
        File.AppendAllText(filePath, logEntry);
    }
}
