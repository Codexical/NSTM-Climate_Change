using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;

public class Sence1 : MonoBehaviour
{
    [SerializeField] private HandLandmarkerRunner _HandTracker;
    [SerializeField] private GameCalibration _gameCalibration;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _title;
    [SerializeField] private GameObject _monster;
    [SerializeField] private GameObject _button;
    [SerializeField] private GameObject _testObject;

    private bool _isGaming = false;

    private void OnEnable()
    {
        _isGaming = false;
        StartCoroutine(WaitToStart());
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
            _testObject.transform.position = new Vector3(x, y, 0);
            // Debug.Log("position: " + position);
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
