using UnityEngine;

public class Scene4 : MonoBehaviour, TimerController
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Timer _timer;

    private void OnEnable()
    {
        _timer.StartTimer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Z))
        {
            _gameManager.SenceChange(5);
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.X))
        {
            _gameManager.SenceChange(6);
        }
    }

    public void TimeOut(int timeOutID)
    {
        _gameManager.SenceChange(1);
    }
}
