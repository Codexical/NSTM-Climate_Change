using UnityEngine;

public class Sence4 : MonoBehaviour, TimerController
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Timer _timer;
    [SerializeField] private Sprite[] _healthList;
    [SerializeField] private Sprite[] _healthFontList;
    [SerializeField] private Sprite[] _bearWin;
    [SerializeField] private GameObject _health;
    [SerializeField] private GameObject _healthFont;
    [SerializeField] private GameObject _bear;

    public void SetBearHealth(int health)
    {
        _health.GetComponent<SpriteRenderer>().sprite = _healthList[health];
        _healthFont.GetComponent<SpriteRenderer>().sprite = _healthFontList[health];
        _bear.GetComponent<SpriteRenderer>().sprite = _bearWin[health];
    }
    private void OnEnable()
    {
        _timer.StartTimer();
    }

    public void TimeOut(int timeOutID)
    {
        _gameManager.SenceChange(1);
    }
}
