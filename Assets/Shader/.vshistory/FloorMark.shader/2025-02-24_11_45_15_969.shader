Shader "Custom/FloorMark"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _RingColor ("Ring Color", Color) = (1, 1, 1, 1)
        _RingRadius("Ring Radius", Float) = 10.0
        _RingRadiusVariance("Ring Radius Variance", Float) = 10.0
        _RingSmoothing("Ring Smoothing", Float) = 10.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _MainTex_ST;
            fixed4 _RingColor;
            float4 _BallPosition;
            float _RingRadius;
            float _RingRadiusVariance;
            float _RingSmoothing;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = v.uv;
                return o;
            }

            float sdfRing(float2 p, float radius, float width)
            {
                return abs(length(p) - radius) - width * 0.5;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 objectPos = _BallPosition.xz;

                float2 fragPos = i.worldPos.xz;

                float dist = distance(objectPos, fragPos);

                float halfRadius = _RingRadius * 0.5;

                float radius = halfRadius + _SinTime.w * 4 * _RingRadiusVariance;
                
                float step = pow( smoothstep(0, radius, dist), _RingSmoothing);

                fixed4 col = lerp( tex2D(_MainTex, i.uv), _RingColor, 1-  step);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}