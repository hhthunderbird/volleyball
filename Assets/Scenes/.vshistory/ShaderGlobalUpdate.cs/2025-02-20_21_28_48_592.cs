using UnityEngine;

public class ShaderGlobalUpdate : MonoBehaviour
{
    void Update()
    {
        Shader.SetGlobalVector( "_ObjectPosition", transform.position );
    }
}
