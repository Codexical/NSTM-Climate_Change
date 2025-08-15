using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;

public class Sence4 : MonoBehaviour, TimerController
{
    [SerializeField] private HandLandmarkerRunner _HandTracker;
    [SerializeField] private GameCalibration _gameCalibration;
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
        yield return new WaitForSeconds(1.0f);
        _isGaming = true;
    }

    void Update()
    {
        if (!_isGaming)
        {
            return;
        }
        List<Vector2> coordinates = _HandTracker.GetResults();
        if (coordinates == null || coordinates.Count < 1)
        {
            return;
        }
        foreach (var coordinate in coordinates)
        {
            var position = _gameCalibration.transformToGameArea(coordinate);
            var x = position[0];
            var y = position[1];
            var buttonRect = _button.GetComponent<RectTransform>();
            if (x >= _button.transform.position.x - buttonRect.sizeDelta.x / 32 &&
                x <= _button.transform.position.x + buttonRect.sizeDelta.x / 32 &&
                y >= _button.transform.position.y - buttonRect.sizeDelta.y / 32 &&
                y <= _button.transform.position.y + buttonRect.sizeDelta.y / 32)
            {
                _gameManager.SenceChange(2);
            }
        }
    }
}
