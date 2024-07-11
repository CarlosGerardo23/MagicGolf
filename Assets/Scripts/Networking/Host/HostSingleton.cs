using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    public HostGameManager HostGameManager { get; private set; }
    private static HostSingleton _instance;
    public static HostSingleton Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindFirstObjectByType<HostSingleton>();
            return _instance;
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
        HostGameManager = new HostGameManager();
    }
}
