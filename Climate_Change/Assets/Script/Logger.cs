using UnityEngine;
using System.IO;

public class Logger : MonoBehaviour
{
    private string dataFolder = "./Data";
    private string nowTime;

    private void Start()
    {
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }
    }
    
    public void SetTime()
    {
        nowTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }

    public void LogData(int[] answers, int[] groundTruth, bool isPass, int score)
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
        string logEntry = $"{nowTime},{taskString}{answerString}{useTime},{isPass},{score}\n";

        string yearMonth = System.DateTime.Now.ToString("yyyyMM");
        string filePath = $"{dataFolder}/{yearMonth}.csv";
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Timestamp,UserAnswer,,,,,,,,,,,,,,,,,,,,UserStatus,,,,,,,,,,,,,,,,,,,,UseTime,IsPass,Score\n");
        }
        File.AppendAllText(filePath, logEntry);
    }
}
