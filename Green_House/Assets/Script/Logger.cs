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
            File.WriteAllText(filePath, "Timestamp,ActivityUserAnswer,,,,,,,,,ActivityUserStatus,,,,,,,,,LifeUserAnswer,,,,,,,,,LifeUserStatus,,,,,,,,,IsFinish,UseTime,IsPass\n");
        }
    }

    public void SetTime()
    {
        nowTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }

    public void LogData(bool isActivity, int[] answers, int[] groundTruth, bool isFinish, float useTime, bool isPass)
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
        string logEntry;
        if (isActivity)
            logEntry = $"{nowTime},{taskString}0,0,0,0,0,0,0,0,0,{answerString}0,0,0,0,0,0,0,0,0,{isFinish},{useTime},{isPass}\n";
        else
            logEntry = $"{nowTime},0,0,0,0,0,0,0,0,0,{taskString}0,0,0,0,0,0,0,0,0,{answerString}{isFinish},{useTime},{isPass}\n";
        File.AppendAllText(filePath, logEntry);
    }
}
