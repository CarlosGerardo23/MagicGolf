using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField _inputField;
   public  async void StartHost()
   {
        await HostSingleton.Instance.HostGameManager.StartHostAsync();
   }

   public async void StartClient()
   {
        await ClientSingleton.Instance.ClientGameManager.StartClientAsync(_inputField.text);
   }
}
