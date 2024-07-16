using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkChatSystem : NetworkBehaviour
{
    [SerializeField] private InputReaderSO _inputReader;
    private UIChat _uiChat;
    private bool _isChatActivated = false;
    public override void OnNetworkSpawn()
    {
        _uiChat = FindFirstObjectByType<UIChat>(FindObjectsInactive.Include);
        _uiChat.AddSendDelegate(SendMessage);
      //  _uiChat.gameObject.SetActive(_isChatActivated);
        _inputReader.onInteractEvent += SetChatState;
        _inputReader.EnablePlayer();
    }
    public override void OnNetworkDespawn()
    {
        _inputReader.onInteractEvent -= SetChatState;
    }


    private void SetChatState()
    {
        // _isChatActivated = !_isChatActivated;
        // _uiChat.gameObject.SetActive(_isChatActivated);
    }

    public void SendMessage()
    {
        SendChatMessageServerRpc(_uiChat.GetMessage());
    }
    private void AddMessage(string message)
    {
        _uiChat.AddMessage(message);
    }
    [ClientRpc]
    private void ReceiveChatMessageClientRpc(string message)
    {
        AddMessage(message);
    }
    [ServerRpc(RequireOwnership = false)]
    private void SendChatMessageServerRpc(string message)
    {
        ReceiveChatMessageClientRpc(message);
    }
}
