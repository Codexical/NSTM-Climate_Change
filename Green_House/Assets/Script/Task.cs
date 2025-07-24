using UnityEngine;

public class Task : MonoBehaviour
{
    [SerializeField] private GameObject _taskMask;
    [SerializeField] private GameObject _correct;
    [SerializeField] private GameObject _wrong;

    public void ShowMask()
    {
        _taskMask.SetActive(true);
        _correct.SetActive(false);
        _wrong.SetActive(false);
    }

    public void HideMask()
    {
        _taskMask.SetActive(false);
    }

    public void ShowCorrect()
    {
        _correct.SetActive(true);
    }
    public void ShowError()
    {
        _wrong.SetActive(true);
    }
}
