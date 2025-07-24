using UnityEngine;

public class Scene2 : MonoBehaviour, TimerController
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Timer _timer;
    private void OnEnable()
    {
        _timer.StartTimer();
    }
    public void TimeOut(int timeOutID)
    {
        _gameManager.SenceChange(3);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A)
        || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W)
        || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.X))
        {
            _gameManager.SenceChange(3);
        }

    }
}
