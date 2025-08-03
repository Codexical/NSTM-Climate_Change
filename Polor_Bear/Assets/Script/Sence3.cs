using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sence3 : MonoBehaviour, TimerController
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Sence4 _sence4;
    [SerializeField] private Bear _bear;
    [SerializeField] private Health _health;
    [SerializeField] private Timer _timer;
    [SerializeField] private Sence3Background _sence3Background;

    private void OnEnable()
    {
        _bear.Init();
        _health.Init();
        _timer.StartTimer();
    }

    public void bearHit(Vector3 position)
    {
        _bear.hit();
        if (_health.bloodLoss())
        {
            _sence3Background.warning();
        }
    }

    public void TimeOut(int timeOutID)
    {
        StopAllCoroutines();
        _gameManager.SenceChange(4);
        _sence4.SetBearHealth(_bear.health());
    }

    public void GameFailed()
    {
        StopAllCoroutines();
        _gameManager.SenceChange(5);
    }

}
