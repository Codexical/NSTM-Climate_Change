using UnityEngine;

public class FinishSence : MonoBehaviour, TimerController
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
}
