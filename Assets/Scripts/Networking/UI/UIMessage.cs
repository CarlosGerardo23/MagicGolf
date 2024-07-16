using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class UIMessage : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _tetx;

    public void SetMessage(string message)
    {
        _tetx.text = message;
    }

}
