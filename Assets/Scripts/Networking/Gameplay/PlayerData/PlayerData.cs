using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerData : NetworkBehaviour
{
    public NetworkVariable<int> PlayerScore = new NetworkVariable<int>();
    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();

    private PlayerDataUI _playerDataUI;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SetPlayerNameServerRpc(ApplicationController.PlayerName);
            PlayerScore.Value = 0;
        }
        _playerDataUI = GetComponent<PlayerDataUI>();
        PlayerName.OnValueChanged += OnPlayerNameChanged;
        UpdatePlayerNameUI(PlayerName.Value.ToString());
    }

    public override void OnNetworkDespawn()
    {
        PlayerName.OnValueChanged -= OnPlayerNameChanged;
    }

    private void OnPlayerNameChanged(FixedString32Bytes oldValue, FixedString32Bytes newValue)
    {
        UpdatePlayerNameUI(newValue.ToString());
    }

    public void AddScore(int value)
    {
        PlayerScore.Value += value;
    }

    [ServerRpc]
    private void SetPlayerNameServerRpc(string playerName)
    {
        PlayerName.Value = playerName;
    }

    private void UpdatePlayerNameUI(string playerName)
    {
        if (_playerDataUI == null)
            _playerDataUI = GetComponent<PlayerDataUI>();

        _playerDataUI.UpdateName(playerName);
    }
}