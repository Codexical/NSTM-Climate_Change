using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    [SerializeField] private GameObject _HomeButton;
    [SerializeField] private Sprite _gameSuccess;
    [SerializeField] private Sprite _gameFailed;
    [SerializeField] private Image _questionImage;
    [SerializeField] private Sprite[] _questionSprites;
    [SerializeField] private Sprite[] _questionCorrectSprites;
    [SerializeField] private Sprite[] _questionErrorSprites;


    public void Hide()
    {
        gameObject.SetActive(false);
        _HomeButton.SetActive(false);
    }

    public void Show(int questionIndex)
    {
        _questionImage.sprite = _questionSprites[questionIndex];
        gameObject.SetActive(true);
    }

    public void ShowError(int questionIndex)
    {
        _questionImage.sprite = _questionErrorSprites[questionIndex];
    }

    public void ShowCorrect(int questionIndex)
    {
        _questionImage.sprite = _questionCorrectSprites[questionIndex];
    }

    public void GameSuccess()
    {
        _questionImage.sprite = _gameSuccess;
        _HomeButton.SetActive(true);
        gameObject.SetActive(true);
    }

    public void GameFailed()
    {
        _questionImage.sprite = _gameFailed;
        gameObject.SetActive(true);
    }

}
