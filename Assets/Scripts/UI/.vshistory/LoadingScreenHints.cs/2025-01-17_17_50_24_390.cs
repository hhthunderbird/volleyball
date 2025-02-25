using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenHints : MonoBehaviour
{
    public string[] hints;
    public Text printedHint;

    private void OnEnable()
    {
        if(hints.Length > 0)
            PrintRandomHint();
    }

    private void PrintRandomHint()
    {
        printedHint.text = hints[ Random.Range( 0, hints.Length ) ];
    }
}
