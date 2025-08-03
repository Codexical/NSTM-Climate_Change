using UnityEngine;

public class Sence5 : MonoBehaviour, TimerController
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Timer _timer;

    private void OnEnable()
    {
        _timer.StartTimer();
    }

    public void TimeOut(int timeOutID)
    {
        _gameManager.SenceChange(1);
    }

}
