using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenHints : MonoBehaviour
{
    public string[] _hints;
    public Text printedHint;

    private void OnEnable()
    {
        if(_hints.Length > 0)
            PrintRandomHint();
    }

    private void PrintRandomHint()
    {
        if ( _hints.Length == 0 ) return;


        printedHint.text = _hints[ Random.Range( 0, _hints.Length ) ];
    }
}
