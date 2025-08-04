using UnityEngine;
using System.Collections.Generic;


public class GameController : MonoBehaviour, TimerController
{
    [SerializeField] private Sence4 _sence4;
    [SerializeField] private QuestionController _questionController;
    [SerializeField] private NoticeController _noticeController;
    [SerializeField] private Score _scoreController;
    [SerializeField] private Timer _gameTimer;
    [SerializeField] private Timer _noticeTimer;
    [SerializeField] private AudioClip _correctSound;
    [SerializeField] private AudioClip _errorSound;
    [SerializeField] private AudioSource _audioSource;
    private int[] answers = { 1, 2, 1, 3, 2, 1, 1, 3, 1, 1, 2, 3, 3, 1, 1, 2, 3, 1, 1, 3 };
    private int[] correctList = { 0, 0, 0, 0, 0 };
    private List<int> answerList = new List<int>();

    private int _questionsCount = 0;
    private int _nowIndex = 0;
    private bool _isNotice = false;

    private void OnEnable()
    {
        _questionsCount = 0;
        _isNotice = false;
        correctList = new int[5];
        answerList = new List<int>();
        _scoreController.ResetScore();
        _gameTimer.Hide();

        _noticeController.Hide();
        _nowIndex = Random.Range(0, answers.Length);
        _questionsCount++;
        answerList.Add(_nowIndex);
        _questionController.SetQuestion(_nowIndex);
        _gameTimer.Show();
        _noticeTimer.Hide();
    }

    private void OnDisable()
    {
        _gameTimer.StopTimer();
        _noticeTimer.StopTimer();
    }

    void Update()
    {
        if (!_isNotice)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                CheckAnswer(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                CheckAnswer(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                CheckAnswer(3);
            }
        }
    }

    private void CheckAnswer(int selected)
    {
        _gameTimer.Hide();
        if (selected == answers[_nowIndex])
        {
            _audioSource.PlayOneShot(_correctSound);
            correctList[_questionsCount - 1] = 1;
            _noticeController.AnswerCorrect();
        }
        else
        {
            _audioSource.PlayOneShot(_errorSound);
            correctList[_questionsCount - 1] = 2;
            _noticeController.AnswerWrong(_nowIndex);
        }
        _isNotice = true;
        _noticeTimer.Show();
        _scoreController.UpdateScore(correctList);
    }

    public void TimeOut()
    {
        if (_isNotice)
        {
            _isNotice = false;
            if (_questionsCount == 5)
            {
                int sum = 0;
                foreach (int val in correctList)
                {
                    if (val == 1)
                    {
                        sum++;
                    }
                }
                _sence4.finishGame(sum);
                return;
            }
            _noticeController.Hide();
            _questionsCount++;
            _nowIndex = Random.Range(0, answers.Length);
            while (answerList.Contains(_nowIndex))
            {
                _nowIndex = Random.Range(0, answers.Length);
            }
            answerList.Add(_nowIndex);
            _questionController.SetQuestion(_nowIndex);
            _gameTimer.Show();
            _noticeTimer.Hide();
        }
        else
        {
            CheckAnswer(-1);
        }
    }
}
