using Unity.Netcode;

public class PlayerData : NetworkBehaviour
{
    public NetworkVariable<int> PlayerScore = new NetworkVariable<int>();

    private string PlayerName;
    private PlayerDataUI _playerDataUI;
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        PlayerName = ApplicationController.PlayerName;
        PlayerScore.Value = 0;
        _playerDataUI= GetComponent<PlayerDataUI>();
        UpdatePlayerNameClientRpc(PlayerName);
    }

    public void AddScore(int value)
    {
        PlayerScore.Value += value;
    }

    [ClientRpc]
    public void UpdatePlayerNameClientRpc(string playerName)
    {
        _playerDataUI.UpdateName(playerName);
    }
}
