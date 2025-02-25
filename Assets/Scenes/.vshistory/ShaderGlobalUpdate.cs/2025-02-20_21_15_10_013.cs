using UnityEngine;

[ExecuteInEditMode]
public class ShaderGlobalUpdate : MonoBehaviour
{
    void Update()
    {
        Shader.SetGlobalVector( transform.position );
    }
}
