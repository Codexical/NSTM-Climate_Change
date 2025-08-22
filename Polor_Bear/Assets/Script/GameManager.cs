using UnityEngine;
using System.IO.Ports;
using System.Collections;

[System.Serializable]
public class Config
{
    public int lowerBound;
    public int upperBound;
    public string sticker;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject[] _sences;
    [SerializeField] private GameObject _calibration;
    [SerializeField] private HandTracker _handTracker;
    private string configPath = "./config.json";
    public Config config = null;

    private void Start()
    {
        for (int i = 0; i < _sences.Length; i++)
        {
            _sences[i].SetActive(false);
        }
        _calibration.SetActive(true);
        LoadConfig();
        StartCoroutine(WaitForCalibration());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            printSticker();
        }
    }

    private IEnumerator WaitForCalibration()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 5000; i++)
        {
            _handTracker.GetClickArea(true);
        }
        Debug.Log("Calibration done");
        _calibration.SetActive(false);
        SenceChange(1);
    }

    public void SenceChange(int senceIndex)
    {
        if (senceIndex < 1 || senceIndex > _sences.Length)
        {
            Debug.LogError("Invalid scene index: " + senceIndex);
            return;
        }
        for (int i = 0; i < _sences.Length; i++)
        {
            if (i == senceIndex - 1)
            {
                _sences[i].SetActive(true);
            }
            else
            {
                _sences[i].SetActive(false);
            }
        }
    }


    public void LoadConfig()
    {
        if (!System.IO.File.Exists(configPath))
        {
            Config defaultConfig = new Config { lowerBound = 0, upperBound = 10000, sticker = "P" };
            string defaultJson = JsonUtility.ToJson(defaultConfig, true);
            System.IO.File.WriteAllText(configPath, defaultJson);
        }
        config = JsonUtility.FromJson<Config>(System.IO.File.ReadAllText(configPath));
        _handTracker.lowerBound = config.lowerBound;
        _handTracker.upperBound = config.upperBound;
    }

    public void printSticker()
    {
        SerialPort sp = null;
        string[] ports = { "COM2", "COM3" };
        foreach (string port in ports)
        {
            try
            {
                sp = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
                sp.Open();
                sp.WriteLine(config.sticker);
                sp.Close();
                Debug.Log("Sending sticker to " + port);
                return;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to open serial port " + port + ": " + e.Message);
            }
        }
    }
}
