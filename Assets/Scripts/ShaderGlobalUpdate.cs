using UnityEngine;

public class ShaderGlobalUpdate : MonoBehaviour
{
    void Update()
    {
        Shader.SetGlobalVector( "_BallPosition", transform.position );
    }
}
