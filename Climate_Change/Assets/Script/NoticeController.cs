using UnityEngine;

public class NoticeController : MonoBehaviour
{
    [SerializeField] private Sprite _correct;
    [SerializeField] private Sprite[] _wrongList;
    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Hide()
    {
        _spriteRenderer.enabled = false;
    }

    public void AnswerCorrect()
    {
        _spriteRenderer.enabled = true;
        _spriteRenderer.sprite = _correct;
    }

    public void AnswerWrong(int index)
    {
        if (index < 0 || index >= _wrongList.Length)
        {
            Debug.LogError("Index out of bounds for QuestionsList.");
            return;
        }
        _spriteRenderer.enabled = true;
        _spriteRenderer.sprite = _wrongList[index];
    }
}
