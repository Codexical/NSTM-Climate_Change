using UnityEngine;
using System.IO;
using System.IO.Ports;

[System.Serializable]
public class Config
{
    public int lowerBound;
    public int upperBound;
    public string sticker;
    public bool resetGround;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject[] _sences;
    [SerializeField] private GameObject _calibration;
    [SerializeField] private HandTracker _handTracker;
    private string configPath = "./config.json";
    private string depthPath = "./depth.txt";
    public Config config = null;

    private void Start()
    {
        for (int i = 0; i < _sences.Length; i++)
        {
            _sences[i].SetActive(false);
        }
        _calibration.SetActive(true);
        LoadConfig();
        _calibration.SetActive(false);
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
            Config defaultConfig = new Config { lowerBound = 0, upperBound = 10000, sticker = "P", resetGround = true};
            string defaultJson = JsonUtility.ToJson(defaultConfig, true);
            System.IO.File.WriteAllText(configPath, defaultJson);
        }
        config = JsonUtility.FromJson<Config>(System.IO.File.ReadAllText(configPath));
        _handTracker.lowerBound = config.lowerBound;
        _handTracker.upperBound = config.upperBound;
        if (config.resetGround)
        {
            Debug.Log("Resetting ground depth");
            for (int i = 0; i < 5000; i++)
            {
                _handTracker.GetClickArea(true);
            }
            using (StreamWriter writer = new StreamWriter("depth.txt", false))
            {
                for (int i = 0; i < _handTracker.imageHeight; i++)
                {
                    for (int j = 0; j < _handTracker.imageWidth; j++)
                    {
                        writer.Write($"{_handTracker.depthDataBufferBase[i * _handTracker.imageWidth + j],5}");
                        if (j < _handTracker.imageWidth - 1) writer.Write(", ");
                    }
                    writer.WriteLine();
                }
            }
            config.resetGround = false;
            System.IO.File.WriteAllText(configPath, JsonUtility.ToJson(config, true));
        }
        else
        {
            Debug.Log("Loading ground depth from config");
            if (System.IO.File.Exists(depthPath))
            {
                string[] lines = System.IO.File.ReadAllLines(depthPath);
                for (int i = 0; i < _handTracker.imageHeight; i++)
                {
                    string[] values = lines[i].Split(new char[] { ',', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < _handTracker.imageWidth; j++)
                    {
                        if (ushort.TryParse(values[j], out ushort depthValue))
                        {
                            _handTracker.depthDataBufferBase[i * _handTracker.imageWidth + j] = depthValue;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Depth file not found: " + depthPath);
            }
        }
        Debug.Log(_handTracker.depthDataBufferBase[0]);
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
