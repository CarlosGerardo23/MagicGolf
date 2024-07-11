using System.Threading.Tasks;

using UnityEngine;

public class ClientSingleton : MonoBehaviour
{
    public ClientGameManager ClientGameManager{get; private set;}
    private static ClientSingleton _instance;
    public static ClientSingleton Instance
    {
        get
        {
            if (_instance != null)
                return _instance;
            _instance = FindFirstObjectByType<ClientSingleton>();
            return _instance;
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public async Task<bool> CreateClient()
    {
        ClientGameManager = new ClientGameManager();
        return await ClientGameManager.InitAsync();
    }
}
