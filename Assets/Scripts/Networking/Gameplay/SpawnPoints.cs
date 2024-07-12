using Unity.Netcode;
using UnityEngine;

public class SpawnPoints : NetworkBehaviour
{
    public NetworkVariable<int> SpawnIndex = new NetworkVariable<int>();
    private int _currentIndex;
    [SerializeField] private Transform[] _point;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnIndex.OnValueChanged += UpdateSpawnIndex;
        }
        UpdateSpawnIndex(0, SpawnIndex.Value);
    }

    private void OnDestroy()
    {
        if (IsServer)
        {
            SpawnIndex.OnValueChanged -= UpdateSpawnIndex;
        }
    }

    private void UpdateSpawnIndex(int previousValue, int newValue)
    {
        _currentIndex = newValue;
    }

    public Vector3 GetPosition()
    {
        if (IsServer)
        {
            int currentSpawnIndex = SpawnIndex.Value;
            SpawnIndex.Value += 1;
            return _point[currentSpawnIndex].position;
        }
        else
        {
            return Vector3.zero; 
        }
    }
}