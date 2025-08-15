using UnityEngine;
using System.IO.Ports;


[System.Serializable]
public class Config
{
    public bool showDebugObjects;
    public string sticker;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _sences;
    [SerializeField] private GameObject[] _debugObjects;
    string configPath = "./config.json";
    public Config config = null;

    private void Start()
    {
        LoadConfig();
        if (config != null)
        {
            foreach (GameObject debugObject in _debugObjects)
            {
                debugObject.SetActive(config.showDebugObjects);
            }
        }
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
            Config defaultConfig = new Config { showDebugObjects = false, sticker = "P" };
            string defaultJson = JsonUtility.ToJson(defaultConfig, true);
            System.IO.File.WriteAllText(configPath, defaultJson);
        }
        config = JsonUtility.FromJson<Config>(System.IO.File.ReadAllText(configPath));
    }

    public void printSticker()
    {
        SerialPort sp = null;
        try
        {
            sp = new SerialPort("COM2", 9600, Parity.None, 8, StopBits.One);
            sp.Open();
            sp.WriteLine(config.sticker);
            sp.Close();
            sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            sp.Open();
            sp.WriteLine(config.sticker);
            sp.Close();
            Debug.Log("Sticker sent: " + config.sticker);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error while sending data: " + e.Message);
        }
    }
}
