using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    private JoinAllocation _allocation;
    private const string MenuScene = "Menu";
    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();

        AuthState authState = await AuthenticationWrapper.DoAuth();
        if (authState == AuthState.Authenticated)
            return true;

        return false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuScene);
    }

    public async Task StartClientAsync(string joinCode)
    {
        try
        {
            _allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return;
        }
        ApplicationController.JoinCode = joinCode;
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(_allocation, "wss");
        transport.SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();
    }
}
