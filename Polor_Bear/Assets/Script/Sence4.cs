using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sence4 : MonoBehaviour, TimerController
{
    [SerializeField] private HandTracker _handTracker;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Timer _timer;
    [SerializeField] private Sprite[] _healthList;
    [SerializeField] private Sprite[] _healthFontList;
    [SerializeField] private Sprite[] _bearWin;
    [SerializeField] private GameObject _health;
    [SerializeField] private GameObject _healthFont;
    [SerializeField] private GameObject _bear;
    [SerializeField] private GameObject _button;
    [SerializeField] private float _requiredTriggerTime = 1.0f;

    private bool _isGaming = false;
    private float _triggerTimer = 0f;

    public void SetBearHealth(int health)
    {
        _health.GetComponent<SpriteRenderer>().sprite = _healthList[health];
        _healthFont.GetComponent<SpriteRenderer>().sprite = _healthFontList[health];
        _bear.GetComponent<SpriteRenderer>().sprite = _bearWin[health];
    }
    private void OnEnable()
    {
        _gameManager.printSticker();
        _timer.StartTimer();
        _isGaming = false;
        StartCoroutine(WaitToStart());
        _triggerTimer = 0f;
        _button.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    private void OnDisable()
    {
        _timer.StopTimer();
    }

    public void TimeOut(int timeOutID)
    {
        _gameManager.SenceChange(1);
    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(1.0f);
        _isGaming = true;
    }

    void Update()
    {
        if (!_isGaming)
        {
            return;
        }

        bool isAreaTrigger = _handTracker.isAreaTriggered(0, -593, 571, 212);
        if (isAreaTrigger)
        {
            _triggerTimer += Time.deltaTime;
            float scale = 1.0f + _triggerTimer / _requiredTriggerTime * 0.1f;
            _button.transform.localScale = new Vector3(scale, scale, scale);
            if (_triggerTimer >= _requiredTriggerTime)
            {
                _gameManager.SenceChange(2);
            }
        }
        else
        {
            _triggerTimer = 0f;
            _button.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
