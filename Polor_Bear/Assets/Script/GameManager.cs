using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameCalibration _gameCalibration;
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private GameObject[] _sences;
    private bool _isCalibrating = false;

    private void Start()
    {
        SenceChange(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !_isCalibrating)
        {
            StartCalibration();
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

    public void StartCalibration()
    {
        _isCalibrating = true;
        _gameScreen.SetActive(false);
        _gameCalibration.StartCalibration();
    }
    public void FinishCalibration()
    {
        _isCalibrating = false;
        _gameScreen.SetActive(true);
        SenceChange(1);
    }
}
