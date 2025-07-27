using UnityEngine;
using System.Collections;
using TMPro;

public class Sence4 : MonoBehaviour, TimerController
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private GameObject _successObject;
    [SerializeField] private TextMeshPro _successTextObject;
    [SerializeField] private GameObject _failedObject;
    [SerializeField] private TextMeshPro _failedTextObject;
    [SerializeField] private AudioClip _successSound;
    [SerializeField] private AudioClip _failedSound;
    [SerializeField] private AudioSource _audioSource;

    private void OnEnable()
    {
        _gameObject.SetActive(true);
        _successObject.SetActive(false);
        _failedObject.SetActive(false);
    }

    public void finishGame(int score)
    {
        _gameObject.SetActive(false);
        if (score >= 3)
        {
            _audioSource.PlayOneShot(_successSound);
            _successObject.SetActive(true);
            _successTextObject.text = $"{score * 20}";
        }
        else
        {
            _audioSource.PlayOneShot(_failedSound);
            _failedObject.SetActive(true);
            _failedTextObject.text = $"{score * 20}";
        }
    }

    public void TimeOut()
    {
        _gameManager.SenceChange(1);
    }
}
