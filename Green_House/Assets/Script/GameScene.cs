using UnityEngine;
using System.IO.Ports;

public class GameScene : MonoBehaviour, TimerController
{
    [SerializeField] private Logger _logger;
    [SerializeField] private TaskController _taskController;
    [SerializeField] private bool _isActivity = false;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Timer _timer;
    [SerializeField] private Timer _noticeTimer;
    [SerializeField] private Timer _timeOutTimer;
    [SerializeField] private Timer _endGameTimer;
    [SerializeField] private Question _questionPanel;
    [SerializeField] private Task[] _tasks;
    public int[] _answers = new int[9];
    [SerializeField] private AudioClip _correctSound;
    [SerializeField] private AudioClip _errorSound;
    [SerializeField] private AudioClip _successSound;
    [SerializeField] private AudioClip _failedSound;
    [SerializeField] private AudioClip _timeOutSound;
    [SerializeField] private AudioSource _audioSource;
    private int[] answerList = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int[] answerStatus = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int _ansCount = 0;
    private int _nowIndex = 0;
    private bool _isSelected = false;
    private bool _isAnswered = false;
    private bool _isFinish = false;
    private bool _isConnect = false;
    public bool _isNotice = false;

    private void OnEnable()
    {
        answerList = new int[9];
        answerStatus = new int[9];
        _ansCount = 0;
        _nowIndex = 0;
        _isSelected = false;
        _isAnswered = false;
        _isFinish = false;
        _isConnect = false;
        _isNotice = false;

        _questionPanel.Hide();
        foreach (var task in _tasks)
        {
            task.ShowMask();
        }
        _noticeTimer.Hide();
        _timer.StartTimer();
        _timeOutTimer.StartTimer();
        _logger.SetTime();
    }

    private void OnDisable()
    {
        _timer.StopTimer();
        _noticeTimer.StopTimer();
        _timeOutTimer.StopTimer();
        _endGameTimer.StopTimer();
    }

    public void TimeOut(int timeOutID)
    {
        if (timeOutID == 2) // Notice timer timeout
        {
            _noticeTimer.Hide();
            _isNotice = false;
            if (_isConnect)
            {
                _logger.LogData(_isActivity, answerStatus, _taskController._answers, true, _timer.GetUseTime(), true);
                _gameManager.printSticker();
                _audioSource.PlayOneShot(_successSound);
                _isFinish = true;
                _timer.StopTimer();
                _questionPanel.GameSuccess();
                _endGameTimer.StartTimer();
                return;
            }
            if (_isFinish || _ansCount >= 9)
            {
                _logger.LogData(_isActivity, answerStatus, _taskController._answers, true, _timer.GetUseTime(), false);
                _audioSource.PlayOneShot(_failedSound);
                _isFinish = true;
                _timer.StopTimer();
                _questionPanel.GameFailed();
                _endGameTimer.StartTimer();
                return;
            }
            _questionPanel.Hide();
            _isSelected = false;
            _isAnswered = false;
        }
        else if (timeOutID == 3)
        {
            if (!_isFinish)
            {
                _isFinish = true;
                if (!_isAnswered && !_isConnect)
                {
                    _logger.LogData(_isActivity, answerStatus, _taskController._answers, false, _timer.GetUseTime(), false);
                    _noticeTimer.Hide();
                    _audioSource.PlayOneShot(_failedSound);
                    _questionPanel.GameFailed();
                    _endGameTimer.StartTimer();
                }
            }
        }
        else if (timeOutID == 4)
        {
            _audioSource.PlayOneShot(_timeOutSound);
        }
        else if (timeOutID == 5)
        {
            if (_isFinish)
            {
                _gameManager.SenceChange(1);
            }
        }

    }

    public bool checkConnected()
    {
        if ((answerList[0] == 1 && answerList[1] == 1 && answerList[2] == 1)
        || (answerList[3] == 1 && answerList[4] == 1 && answerList[5] == 1)
        || (answerList[6] == 1 && answerList[7] == 1 && answerList[8] == 1)
        || (answerList[0] == 1 && answerList[3] == 1 && answerList[6] == 1)
        || (answerList[1] == 1 && answerList[4] == 1 && answerList[7] == 1)
        || (answerList[2] == 1 && answerList[5] == 1 && answerList[8] == 1)
        || (answerList[0] == 1 && answerList[4] == 1 && answerList[8] == 1)
        || (answerList[2] == 1 && answerList[4] == 1 && answerList[6] == 1)
        )
        {
            return true;
        }
        return false;
    }

    public void CheckAnswer(int answerIndex)
    {
        if (_nowIndex < 1 || _nowIndex > 9)
        {
            Debug.LogError("Invalid task index: " + _nowIndex);
            return;
        }
        _tasks[_nowIndex - 1].HideMask();
        Debug.Log("Checking answer: " + answerIndex);
        _ansCount++;
        _isAnswered = true;
        var taskIdx = _questionPanel._questionImage.sprite.name[^3] - '0';
        answerStatus[taskIdx - 1] = answerIndex;
        if (answerIndex == _answers[_nowIndex - 1])
        {
            _audioSource.PlayOneShot(_correctSound);
            answerList[_nowIndex - 1] = 1;
            _questionPanel.ShowCorrect(_nowIndex - 1);
            _tasks[_nowIndex - 1].ShowCorrect();
        }
        else
        {
            _audioSource.PlayOneShot(_errorSound);
            answerList[_nowIndex - 1] = -1;
            _questionPanel.ShowError(_nowIndex - 1);
            _tasks[_nowIndex - 1].ShowError();
        }
        if (checkConnected())
        {
            _isConnect = true;
        }
        _noticeTimer.Show();
        _isNotice = true;
    }

    public void SelectTask(int taskIndex)
    {
        if (!_isSelected && !_isFinish)
        {
            Debug.Log("Task selected: " + taskIndex);
            _isSelected = true;
            _nowIndex = taskIndex;
            _questionPanel.Show(taskIndex - 1);
        }
    }

    void Update()
    {
        if (_isSelected && !_isAnswered)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CheckAnswer(1);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                CheckAnswer(2);
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                CheckAnswer(3);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                CheckAnswer(4);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                CheckAnswer(5);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                CheckAnswer(6);
            }
        }
        else if (_isNotice && _gameManager.isKeySkipEnabled() && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.X)))
        {
            TimeOut(2);
        }
    }
}
