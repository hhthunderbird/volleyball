using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyCard : NetworkBehaviour
{
    [SerializeField] private LobbyScreen _lobby;

    [SerializeField] private TMP_Text _nameField;
    public string Name
    {
        set => _nameField.text = value;
    }

    [SerializeField] private Slider _sideSlideToggle;

    public Side SideToggle
    {
        set => _sideSlideToggle.value = value == Side.Left ? 0 : 1;
    }

    private void Start()
    {
        _sideSlideToggle.onValueChanged.AddListener( OnSideChange );
    }

    private void OnSideChange( float value )
    {
        var side = ((Side) (int)value);
        _lobby.OnPlayerSideChange( NetworkManager.Singleton.LocalClientId, side );
    }
}
