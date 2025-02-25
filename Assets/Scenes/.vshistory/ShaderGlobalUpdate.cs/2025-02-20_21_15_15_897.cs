using UnityEngine;

[ExecuteInEditMode]
public class ShaderGlobalUpdate : MonoBehaviour
{
    void Update()
    {
        Shader.SetGlobalVector("_Location", transform.position );
    }
}
