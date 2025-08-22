using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sence1 : MonoBehaviour
{
    [SerializeField] private HandTracker _handTracker;
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

        var coordinates = _handTracker.GetClickArea();
        if (coordinates == null)
        {
            return;
        }

        int posX = 0;
        int posY = -326;
        int width = 1265;
        int height = 281;

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
