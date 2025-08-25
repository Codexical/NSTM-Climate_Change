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
    private bool _isGaming = false;

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
        yield return new WaitForSeconds(2.0f);
        _isGaming = true;
    }

    void Update()
    {
        if (!_isGaming)
        {
            return;
        }

        var coordinates = _handTracker.GetClickArea();
        if (coordinates == null)
        {
            return;
        }

        int posX = 0;
        int posY = -593;
        int width = 571;
        int height = 212;

        int offsetX = 320;
        int offsetY = 180;
        int minX = (posX - width / 2) * 640 / 3413;
        int maxX = (posX + width / 2) * 640 / 3413;
        int minY = (posY - height / 2) * 360 / 1920;
        int maxY = (posY + height / 2) * 360 / 1920;

        bool isAreaTrigger = false;
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if (coordinates[y + offsetY, x + offsetX])
                {
                    isAreaTrigger = true;
                    break;
                }
            }
        }
        if (isAreaTrigger)
        {
            _gameManager.SenceChange(2);
        }
    }
}
