using UnityEngine;
using System.Collections;
using TMPro;
using System.IO.Ports;

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
            try
            {
                SerialPort sp = new SerialPort("COM2", 9600, Parity.None, 8, StopBits.One);
                sp.Open();
                sp.WriteLine("P");
                sp.Close();
                sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
                sp.Open();
                sp.WriteLine("P");
                sp.Close();
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error while sending data: " + e.Message);
            }
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
