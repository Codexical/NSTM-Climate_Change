using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;

public class Sence5 : MonoBehaviour, TimerController
{
    [SerializeField] private HandLandmarkerRunner _HandTracker;
    [SerializeField] private GameCalibration _gameCalibration;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Timer _timer;
    [SerializeField] private GameObject _button;
    private bool _isGaming = false;

    private void OnEnable()
    {
        _timer.StartTimer();
        _isGaming = false;
        StartCoroutine(WaitToStart());
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
