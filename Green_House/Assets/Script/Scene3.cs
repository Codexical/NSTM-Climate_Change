using UnityEngine;

public class Scene3 : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A)
        || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W)
        || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.X))
        {
            _gameManager.SenceChange(4);
        }
    }
}
