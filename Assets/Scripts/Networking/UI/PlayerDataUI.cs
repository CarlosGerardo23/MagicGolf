using Unity.Netcode;
using UnityEngine;

public class PlayerDataUI : NetworkBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI _playerNameText;
    [SerializeField] private TMPro.TextMeshProUGUI _currentPointsText;
    private PlayerData _playerData;


    public override void OnNetworkSpawn()
    {
        if (!IsClient) return;
        UpdateName(ApplicationController.PlayerName);
        _playerData = GetComponent<PlayerData>();
        _playerData.PlayerScore.OnValueChanged += UpdateScore;
        SetPlayerNameServerRpc(ApplicationController.PlayerName);
        UpdateScore(0, _playerData.PlayerScore.Value);
    }

    public void UpdateName(string newValue)
    {
        if(!IsClient)return;
        _playerNameText.text = newValue;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient) return;
        _playerData.PlayerScore.OnValueChanged -= UpdateScore;
    }

    private void UpdateScore(int previousValue, int newValue)
    {
        _currentPointsText.text = newValue.ToString();

    }
     [ServerRpc]
    public void SetPlayerNameServerRpc(string name)
    {
        if(IsClient)return;
          _playerNameText.text = name;
    }
}
