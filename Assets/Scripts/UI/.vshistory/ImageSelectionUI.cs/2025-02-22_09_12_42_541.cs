using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectionUI : MonoBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Button _confirmGameButton;
    [SerializeField] private Text _playersCountText;

    public event Action OnConfirm;

    private void Awake()
    {
        _confirmGameButton.onClick.AddListener( ConfirmButton );
    }

    private void ConfirmButton()
    {
        SessionManager.Instance.Startgame();
    }



}