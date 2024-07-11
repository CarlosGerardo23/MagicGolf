using System;
using Unity.Netcode;
using UnityEngine;

public class SpawnPoints : NetworkBehaviour
{
    public NetworkVariable<int> SpawnIndex = new NetworkVariable<int>();
    public int _currentIndex;
    [SerializeField] private Transform[] _point;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        SpawnIndex.Value = 0;
        _currentIndex=SpawnIndex.Value;
        SpawnIndex.OnValueChanged+= UpdateSpawnIndex;
    }

    private void UpdateSpawnIndex(int previousValue, int newValue)
    {
        _currentIndex=newValue;
    }

    public Vector3 GetPosition()
    {
        SpawnIndex.Value+=1;
        return _point[_currentIndex].position;
    }

    
}
