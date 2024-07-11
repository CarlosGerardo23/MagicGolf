using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    public static string JoinCode { get; set; }
    public static string PlayerName { get; private set; }
    [SerializeField] private ClientSingleton _clientSingletonPrefab;
    [SerializeField] private HostSingleton _hostSingletonPrefab;
    [SerializeField] private TMPro.TMP_InputField _playerNameInputField;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public async void LaunchGame()
    {
        await Launch();
    }
    private async Task Launch()
    {
        HostSingleton hostSingleton = Instantiate(_hostSingletonPrefab);
        hostSingleton.CreateHost();

        ClientSingleton clientSigletonInstance = Instantiate(_clientSingletonPrefab);
        bool authenticated = await clientSigletonInstance.CreateClient();


        if (authenticated)
        {
            PlayerName = _playerNameInputField.text;
            clientSigletonInstance.ClientGameManager.GoToMenu();
        }

    }
}
