using UnityEngine;
using System.IO.Ports;

[System.Serializable]
public class Config
{
    public Vector2 top;
    public Vector2 left;
    public Vector2 right;
    public Vector2 bottom;
    public bool keepHand;
    public string sticker;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameCalibration _gameCalibration;
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject[] _sences;
    private bool _isCalibrating = false;
    private bool _isBegin = false;
    private string configPath = "./config.json";
    public Config config = null;

    private void Start()
    {
        LoadConfig();
        _isBegin = true;
        SenceChange(1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            printSticker();
        }
    }

    public void SenceChange(int senceIndex)
    {
        if (senceIndex < 1 || senceIndex > _sences.Length)
        {
            Debug.LogError("Invalid scene index: " + senceIndex);
            return;
        }
        if (senceIndex == 1)
        {
            _isBegin = true;
        }
        else
        {
            _isBegin = false;
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

    public void StartCalibration()
    {
        if (_isBegin)
        {
            _isCalibrating = true;
            _gameScreen.SetActive(false);
            _background.SetActive(false);
            _gameCalibration.StartCalibration();
        }
    }
    public void FinishCalibration()
    {
        _isCalibrating = false;
        _background.SetActive(true);
        _gameScreen.SetActive(true);
        SenceChange(1);
    }

    public void LoadConfig()
    {
        if (!System.IO.File.Exists(configPath))
        {
            Config defaultConfig = new Config { top = { x = 0, y = 10 }, left = { x = -10, y = 0 }, right = { x = 10, y = 0 }, bottom = { x = 0, y = -10 }, keepHand = false, sticker = "P" };
            string defaultJson = JsonUtility.ToJson(defaultConfig, true);
            System.IO.File.WriteAllText(configPath, defaultJson);
        }
        config = JsonUtility.FromJson<Config>(System.IO.File.ReadAllText(configPath));
    }

    public void saveConfig(float topX, float topY, float leftX, float leftY, float rightX, float rightY, float bottomX, float bottomY)
    {
        Config newConfig = new Config
        {
            top = new Vector2(topX, topY),
            left = new Vector2(leftX, leftY),
            right = new Vector2(rightX, rightY),
            bottom = new Vector2(bottomX, bottomY),
            keepHand = config.keepHand,
            sticker = config.sticker
        };
        string json = JsonUtility.ToJson(newConfig, true);
        System.IO.File.WriteAllText(configPath, json);
        config = newConfig;
        Debug.Log("Configuration saved to " + configPath);
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
