using NUnit.Framework.Internal;
using Unity.Burst.CompilerServices;
using Unity.Netcode;
using static UnityEditor.ShaderData;
using static UnityEngine.InputManagerEntry;
using Unity.Services.Lobbies.Models;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerLobbyState : NetworkBehaviour
{
    private string[] _adjs = new string[]
    {
        ,"Sparkling"
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
    }
}
