Shader "Custom/ShadowBlob"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Tint("Tint", Color) = (1, 1, 1, 1) 
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha  
        ZWrite Off
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
                float alpha = tex2D(_MainTex, i.uv).a;
                
                //float white = texColor.r; 

                // Apply tint to the white area
                fixed3 color = _Tint.rgb * white;

                return fixed4(color, white * _Tint.a); // Keep tint alpha controllable
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
