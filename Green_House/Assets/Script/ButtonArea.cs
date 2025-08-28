using UnityEngine;
using UnityEngine.UI;


public class ButtonArea : MonoBehaviour
{
    [SerializeField] private Button button;


    private void OnMouseUpAsButton()
    {
        button.onClick.Invoke();
    }
}
