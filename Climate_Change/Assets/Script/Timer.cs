using UnityEngine;
using System.Collections;
using TMPro;

public interface TimerController
{
    void TimeOut();
}

public class Timer : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _parentComponent;
    [SerializeField] private TextMeshPro _timerText;
    [SerializeField] private float _timeLimit = 10f;
    private TimerController _parent;
    private float _timeRemaining;
    private bool _isTimerRunning = false;

    public void Show()
    {
        gameObject.SetActive(true);
        StartTimer();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        StopTimer();
    }

    public void StartTimer()
    {
        _timeRemaining = _timeLimit + 1f;
        _isTimerRunning = true;
    }
    public void StopTimer()
    {
        _isTimerRunning = false;
    }

    public int GetUseTime()
    {
        return (int)(_timeLimit - _timeRemaining + 1f);
    }

    private void Update()
    {
        if (_isTimerRunning)
        {
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining <= 1f)
            {
                _parent = _parentComponent as TimerController;
                _parent.TimeOut();
                _isTimerRunning = false;
            }
            else
            {
                UpdateTimerText();
            }
        }
    }
    private void UpdateTimerText()
    {
        _timerText.text = $"{Mathf.FloorToInt(_timeRemaining)}";
    }
}
