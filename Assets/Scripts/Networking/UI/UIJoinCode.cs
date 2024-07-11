using UnityEngine;

public class UIJoinCode : MonoBehaviour
{
    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private TMPro.TextMeshProUGUI _text;
    void Start()
    {
        _text.text = ApplicationController.JoinCode;
    }

    public void HideLoadingCanvas()
    {
        _loadingCanvas.SetActive(false);
    }
}
