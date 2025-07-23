using UnityEngine;

public class Sence3 : MonoBehaviour, TimerController
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Timer _timer;

    private void OnEnable()
    {
        _timer.StartTimer();
    }

    public void TimeOut()
    {
        _gameManager.SenceChange(4);
    }
}
