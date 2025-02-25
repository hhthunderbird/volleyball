using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyCard : NetworkBehaviour
{
    [SerializeField] private LobbyScreen _lobby;

    private static string[] _adjs = new string[]
    {
        "Sparkling"
        ,"Fuzzy"
        ,"Glittering"
        ,"Whimsical"
        ,"Radiant"
        ,"Mystic"
        ,"Jolly"
        ,"Zesty"
        ,"Bouncy"
        ,"Snazzy"
        ,"Cosmic"
        ,"Fluffy"
        ,"Wobbly"
        ,"Sassy"
        ,"Giggly"
        ,"Dizzy"
        ,"Peppy"
        ,"Cheery"
        ,"Snappy"
        ,"Breezy"
        ,"Quirky"
        ,"Silly"
        ,"Wacky"
        ,"Groovy"
        ,"Spunky"
        ,"Blazing"
        ,"Chilly"
        ,"Sunny"
        ,"Rusty"
        ,"Shiny"
    };


    private static string[] _noums = new string[]
    {
        "Panda"
        ,"Rocket"
        ,"Cupcake"
        ,"Ninja"
        ,"Unicorn"
        ,"Dragon"
        ,"Koala"
        ,"Pickle"
        ,"Waffle"
        ,"Penguin"
        ,"Llama"
        ,"Marshmallow"
        ,"Giraffe"
        ,"Bubble"
        ,"Tornado"
        ,"Comet"
        ,"Jellybean"
        ,"Hedgehog"
        ,"Pancake"
        ,"Narwhal"
        ,"Donut"
        ,"Starfish"
        ,"Peacock"
        ,"Cactus"
        ,"Moonbeam"
        ,"Thunder"
        ,"Popcorn"
        ,"Firefly"
        ,"Snowflake"
        ,"Pineapple"
    };
    public static string RandomName
    {
        get
        {
            var adjIndex = UnityEngine.Random.Range( 0, _adjs.Length );
            var noumIndex = UnityEngine.Random.Range( 0, _noums.Length );
            return $"{_adjs[ adjIndex ]} {_noums[noumIndex]}";
        }
    }

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
        var side = (SideToggle (int)value);
        _lobby.OnPlayerSideChange(NetworkManager.Singleton.LocalClientId)
    }
}
