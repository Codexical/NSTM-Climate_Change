using UnityEngine;

public class Score : MonoBehaviour
{

    [SerializeField] private Sprite _correctSprites;
    [SerializeField] private Sprite _wrongSprites;
    [SerializeField] private Sprite _InitSprites;
    [SerializeField] private GameObject[] _scoreObjects;

    public void UpdateScore(int[] correctList)
    {
        for (int i = 0; i < correctList.Length; i++)
        {
            if (correctList[i] == 1)
            {
                _scoreObjects[i].GetComponent<SpriteRenderer>().sprite = _correctSprites;
            }
            else if (correctList[i] == 2)
            {
                _scoreObjects[i].GetComponent<SpriteRenderer>().sprite = _wrongSprites;
            }
        }
    }

    public void ResetScore()
    {
        foreach (var scoreObject in _scoreObjects)
        {
            scoreObject.GetComponent<SpriteRenderer>().sprite = _InitSprites;
        }
    }
}
