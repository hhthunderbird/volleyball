using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyCard : NetworkBehaviour
{
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

    [SerializeField] private Text _nameField;

    [SerializeField] private Slider _sideSlideToggle;
}
