using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class UIChat : MonoBehaviour
{
    [SerializeField] private UIMessage _messagePrefab;
    [SerializeField] private TMPro.TMP_InputField _messageInputField;

    [SerializeField] private Transform _chatMessageParent;
    [SerializeField] private Button _sendButton;

    public void AddSendDelegate(UnityAction sendEvent)
    {
        _sendButton.onClick.AddListener(sendEvent);
    }

    public void AddMessage(string message)
    {
        UIMessage uIMessage = Instantiate(_messagePrefab);
        uIMessage.SetMessage(message);
        uIMessage.transform.SetParent(_chatMessageParent);
    }
    public string GetMessage()
    {
        string message = _messageInputField.text;
        _messageInputField.text = "";
        return message;
    }
}
