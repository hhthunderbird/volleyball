using Unity.Netcode;

public class PlayerLobbyState : NetworkBehaviour
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

    public static string Adjective
    {
        get
        {
            var index = UnityEngine.Random.Range( 0, _adjs.Length );
            return _adjs[ index ];
        }
    }

    private string[] _noums = new string[]
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
}
