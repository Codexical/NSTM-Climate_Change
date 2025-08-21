using UnityEngine;

public class Scene3 : MonoBehaviour, TimerController
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Timer _timer;

    private void OnEnable()
    {
        _timer.StartTimer();
    }

    private void OnDisable()
    {
        _timer.StopTimer();
    }

    void Update()
    {
        if (_gameManager.isKeySkipEnabled() &&
          (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A)
        || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W)
        || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.X)))
        {
            _gameManager.SenceChange(3);
        }
    }

    public void TimeOut(int timeOutID)
    {
        _gameManager.SenceChange(1);
    }
}
