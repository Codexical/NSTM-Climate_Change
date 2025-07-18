using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshPro _timerText;
    [SerializeField] private float _timeLimit = 10f;
    private Sence3 _parent;
    private float _timeRemaining;
    private bool _isTimerRunning = false;

    public void StartTimer()
    {
        _timeRemaining = _timeLimit;
        _isTimerRunning = true;
    }

    public void StopTimer()
    {
        _timeRemaining = _timeLimit;
        _isTimerRunning = false;
    }

    void Start()
    {
        _parent = GetComponentInParent<Sence3>();
        _timeRemaining = _timeLimit;
        _isTimerRunning = true;
        UpdateTimerText();
    }

    void Update()
    {
        if (_isTimerRunning)
        {
            _timeRemaining -= Time.deltaTime;

            if (_timeRemaining <= 0f)
            {
                _timeRemaining = 0f;
                _isTimerRunning = false;
                _parent.GameSuccess();
            }
            UpdateTimerText();
        }
    }
    private void UpdateTimerText()
    {
        _timerText.text = $"{_timeRemaining:F0}";
    }
}
