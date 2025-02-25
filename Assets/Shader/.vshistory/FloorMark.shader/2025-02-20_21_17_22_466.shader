Shader "Unlit/FloorMark"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Location("Location", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct meshdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Location;

            interpolator vert (meshdata v)
            {
                interpolator i;
                i.vertex = UnityObjectToClipPos(v.vertex);
                i.worldPos = mul(unity_ObjectToWorld, v.vertex);
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return i;
            }

            fixed4 frag (interpolator i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float dist = saturate( distance(_Location.xyz, i.worldPos) );
                return col * dist;
            }
            ENDCG
        }
    }
}
