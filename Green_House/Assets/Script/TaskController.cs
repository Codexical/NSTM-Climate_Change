using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskController : MonoBehaviour
{
    [SerializeField] private GameScene _gameScene;
    [SerializeField] private Question _questionPanel;
    [SerializeField] private GameObject _answerObjects;
    [SerializeField] private GameObject[] _taskObjects;
    [SerializeField] private Sprite[] _taskQuestionSprites;
    [SerializeField] private Sprite[] _taskCorrectSprites;
    [SerializeField] private Sprite[] _taskWrongSprites;
    [SerializeField] private Sprite[] _taskButtonSprites;
    [SerializeField] public int[] _answers;
    private int[] taskList = new int[9];
    private string keyboardList = "QAZWSX";
    private void OnEnable()
    {
        RandomizeTasks();
        for (int i = 0; i < taskList.Length; i++)
        {
            int taskIndex = taskList[i] - 1;
            // _answers[i] = taskIndex;
            _gameScene._answers[i] = _answers[taskIndex];
            _taskObjects[i].GetComponent<Image>().sprite = _taskButtonSprites[taskIndex];
            _questionPanel._questionSprites[i] = _taskQuestionSprites[taskIndex];
            _questionPanel._questionCorrectSprites[i] = _taskCorrectSprites[taskIndex];
            _questionPanel._questionErrorSprites[i] = _taskWrongSprites[taskIndex];
        }
        var answerPanel = _answerObjects.GetComponent<TextMeshPro>();
        answerPanel.text = keyboardList[_answers[taskList[0] - 1] - 1].ToString() +
                            keyboardList[_answers[taskList[3] - 1] - 1].ToString() +
                            keyboardList[_answers[taskList[6] - 1] - 1].ToString() +
                            keyboardList[_answers[taskList[1] - 1] - 1].ToString() +
                            keyboardList[_answers[taskList[4] - 1] - 1].ToString() +
                            keyboardList[_answers[taskList[7] - 1] - 1].ToString() +
                            keyboardList[_answers[taskList[2] - 1] - 1].ToString() +
                            keyboardList[_answers[taskList[5] - 1] - 1].ToString() +
                            keyboardList[_answers[taskList[8] - 1] - 1].ToString();

    }

    private void RandomizeTasks()
    {
        taskList = new int[9];
        for (int i = 0; i < taskList.Length; i++)
        {
            int num;
            do
            {
                num = Random.Range(1, 10);
            } while (System.Array.IndexOf(taskList, num) != -1);
            taskList[i] = num;
        }
    }
}
