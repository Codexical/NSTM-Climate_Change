using UnityEngine;

public class Scene1 : MonoBehaviour, TimerController
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Timer _timer;
    [SerializeField] private GameObject _button;
    [SerializeField] private GameObject _timerObject;
    [SerializeField] private GameObject _description1;
    [SerializeField] private GameObject _description2;

    bool _isLoaded = false;
    private void OnEnable()
    {
        _isLoaded = false;
        _button.SetActive(true);
        _timerObject.SetActive(false);
        _description1.SetActive(true);
        _description2.SetActive(false);
        _timer.StopTimer();
    }

    public void LoadInfo()
    {
        _isLoaded = true;
        _button.SetActive(false);
        _timerObject.SetActive(true);
        _description1.SetActive(false);
        _description2.SetActive(true);
        _timer.StartTimer();
    }
    public void TimeOut(int timeOutID)
    {
        _gameManager.SenceChange(2);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A)
        || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W)
        || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.X))
        {
            if (_isLoaded)
                _gameManager.SenceChange(2);
            else
                LoadInfo();
        }
    }
}
