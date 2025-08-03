using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;

public class GameCalibration : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private HandLandmarkerRunner _HandTracker;
    [SerializeField] private GameObject _Hand;
    [SerializeField] private GameObject _gameArea;
    [SerializeField] private GameObject[] _calibrationObjects;
    private bool _isCalibrating = false;

    private void Start()
    {
        foreach (var obj in _calibrationObjects)
        {
            obj.SetActive(false);
        }
        _Hand.SetActive(false);
        _gameArea.SetActive(false);
    }

    private void Update()
    {
        if (_isCalibrating)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _isCalibrating = false;
                foreach (var obj in _calibrationObjects)
                {
                    obj.SetActive(false);
                }
                _Hand.SetActive(false);
                _gameArea.SetActive(false);
                Debug.Log("Calibration completed.");
                _gameManager.FinishCalibration();
            }

            List<Vector2> coordinates = _HandTracker.GetResults();
            if (coordinates == null || coordinates.Count < 1)
            {
                return;
            }
            var x = (0.5f - coordinates[0].x) * 3413 / 16;
            var y = (0.5f - coordinates[0].y) * 1920 / 16;
            if (Input.GetKey(KeyCode.W))
            {
                _calibrationObjects[0].transform.position = new Vector3(x, y, 0);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _calibrationObjects[1].transform.position = new Vector3(x, y, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _calibrationObjects[2].transform.position = new Vector3(x, y, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _calibrationObjects[3].transform.position = new Vector3(x, y, 0);
            }
            var center_x = 0f;
            var center_y = 0f;
            foreach (var obj in _calibrationObjects)
            {
                center_x += obj.transform.position.x;
                center_y += obj.transform.position.y;
            }
            center_x /= 4;
            center_y /= 4;
            var width = (_calibrationObjects[2].transform.position.x - _calibrationObjects[1].transform.position.x) * 16;
            var height = (_calibrationObjects[0].transform.position.y - _calibrationObjects[3].transform.position.y) * 16;
            _gameArea.transform.position = new Vector3(center_x, center_y, 0);
            _gameArea.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }
    }

    public void StartCalibration()
    {
        Debug.Log("Starting calibration...");
        _isCalibrating = true;
        foreach (var obj in _calibrationObjects)
        {
            obj.SetActive(true);
        }
        _Hand.SetActive(true);
        _gameArea.SetActive(true);
    }

    public Vector2 transformToGameArea(Vector2 coordinate)
    {
        var world_x = (0.5f - coordinate.x) * 3413 / 16;
        var world_y = (0.5f - coordinate.y) * 1920 / 16;

        var relative_x = world_x - _gameArea.transform.position.x;
        var relative_y = world_y - _gameArea.transform.position.y;

        var gameAreaWidth = _gameArea.GetComponent<RectTransform>().sizeDelta.x;
        var gameAreaHeight = _gameArea.GetComponent<RectTransform>().sizeDelta.y;

        var normalized_x = relative_x / gameAreaWidth;
        var normalized_y = relative_y / gameAreaHeight;

        var final_x = normalized_x * 1920;
        var final_y = normalized_y * 1920;

        return new Vector2(final_x, final_y);

    }
}
