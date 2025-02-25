using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenHints : MonoBehaviour
{
    [SerializeField] private string[] _hints;
    [SerializeField] private Text printedHint;

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
