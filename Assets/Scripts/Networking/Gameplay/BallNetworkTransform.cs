using Unity.Netcode.Components;
using UnityEngine;

public class BallNetworkTransform : NetworkTransform
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }

    protected override void Update()
    {
        CanCommitToTransform = IsOwner;
        if (NetworkManager != null)
        {
            if (NetworkManager.IsConnectedClient || NetworkManager.IsListening)
            {
                if (CanCommitToTransform)
                    TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                
            }
        }
        base.Update();

    }
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
