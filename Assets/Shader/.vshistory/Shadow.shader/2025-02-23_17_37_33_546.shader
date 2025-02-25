Shader "Custom/ShadowBlob"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Tint("Tint", Color) = (0, 0, 0, 0.5) // Default dark gray with some transparency
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend DstColor SrcAlpha  // Darkens the background based on alpha
        ZWrite Off               // Do not write to the depth buffer
        Cull Off                 // Render both sides
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Tint;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed alpha = texColor.a * _Tint.a; // Multiply texture alpha with Tint alpha
                fixed3 color = _Tint.rgb * texColor.rgb; // Multiply tint color with texture color

                return fixed4(color, alpha);
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
