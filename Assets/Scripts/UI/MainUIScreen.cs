using UnityEngine;

public class MainUIScreen : MonoBehaviour
{
    void Awake()
    {
        UIScreen.Focus( GetComponent<UIScreen>() );
    }
}
