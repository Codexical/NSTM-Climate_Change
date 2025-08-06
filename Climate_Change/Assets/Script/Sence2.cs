using UnityEngine;

public class Sence2 : MonoBehaviour, TimerController
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Timer _timer;

    private void OnEnable()
    {
        _timer.StartTimer();
    }
    private void OnDisable()
    {
        _timer.StopTimer();
    }

    public void TimeOut()
    {
        _gameManager.SenceChange(1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            _gameManager.SenceChange(3);
        }

    }
}
