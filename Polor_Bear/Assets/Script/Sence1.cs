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
    [SerializeField] private float _requiredTriggerTime = 1.0f;

    private bool _isGaming = false;
    private float _triggerTimer = 0f;

    private void OnEnable()
    {
        _isGaming = false;
        StartCoroutine(WaitToStart());
        _triggerTimer = 0f;
        _button.transform.localScale = new Vector3(1f, 1f, 1f);
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

        bool isAreaTrigger = _handTracker.isAreaTriggered(0, -326, 1265, 281);
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
