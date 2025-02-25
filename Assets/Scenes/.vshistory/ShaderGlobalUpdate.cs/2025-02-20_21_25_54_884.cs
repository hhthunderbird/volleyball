using UnityEngine;

[ExecuteInEditMode]
public class ShaderGlobalUpdate : MonoBehaviour
{
    void Update()
    {
        Shader.SetGlobalVector( "_ObjectPosition", transform.position );
    }
}
