using Unity.Netcode;
using UnityEngine;

public class GemController : NetworkBehaviour
{
    [SerializeField] private GameObject _gemObject;
    private bool _collected;

    public override void OnNetworkSpawn()

    {
        if (!IsServer) return;
        _collected = false;
    }
    public int Collect()
    {
        if (!IsServer || _collected)
        {
            ShowStatus(false);
            return 0;
        }

        RequestDeactivation();
        _collected = true;
        ShowStatus(false);
        return 5;
    }

    private void ShowStatus(bool state)
    {
        _gemObject.SetActive(state);
    }
    [ClientRpc]
    public void DeactivateGameObjectClientRpc()
    {
        ShowStatus(false);
    }

    public void RequestDeactivation()
    {
        DeactivateGameObjectClientRpc();
    }
}
