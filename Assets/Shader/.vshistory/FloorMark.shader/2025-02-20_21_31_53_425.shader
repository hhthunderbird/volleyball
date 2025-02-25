Shader "Custom/FloorMark"
{
    Properties
    {
        _Color ("Ring Color", Color) = (1, 1, 1, 1)
        _RingWidth ("Ring Width", Float) = 0.1
        _RingRadius("Ring Radius", Float) = 10.0
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

            fixed4 _Color;
            float _RingWidth;
            float4 _ObjectPosition;
            float _RingRadius;

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
                float2 objectPos = _ObjectPosition.xz;
                float2 fragPos = i.worldPos.xz;

                // Calculate the distance from the object
                float2 delta = fragPos - objectPos;
                float distance = length(delta);

                // Define the ring radius and width
                float ringRadius = _FloorSize * 0.5; // Adjust as needed
                float ringWidth = _RingWidth;

                // Calculate the SDF for the ring
                float sdf = sdfRing(delta, ringRadius, ringWidth);

                // Smoothstep to create a smooth ring
                float ring = smoothstep(0.0, 0.02, -sdf);

                // Apply the ring color
                fixed4 col = _Color * ring;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}