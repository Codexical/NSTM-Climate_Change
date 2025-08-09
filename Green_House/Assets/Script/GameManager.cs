using UnityEngine;


[System.Serializable]
public class Config
{
    public bool showDebugObjects;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _sences;
    [SerializeField] private GameObject[] _debugObjects;
    string configPath = "./config.json";

    private void Start()
    {
        if (!System.IO.File.Exists(configPath))
        {
            // Create default config file with showDebugObjects = false
            Config defaultConfig = new Config { showDebugObjects = false };
            string defaultJson = JsonUtility.ToJson(defaultConfig, true);
            System.IO.File.WriteAllText(configPath, defaultJson);
        }
        string json = System.IO.File.ReadAllText(configPath);
        Config config = JsonUtility.FromJson<Config>(json);
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
}
