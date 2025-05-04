Shader "Unlit/skybox_shader"
{
      Properties
    {
        _TopColor("Top Color", Color) = (0.3, 0.6, 1.0, 1)
        _BottomColor("Bottom Color", Color) = (0.8, 0.6, 1.0, 1)
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _TopColor;
            fixed4 _BottomColor;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 direction : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.direction = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 dir = normalize(i.direction);
                float t = saturate(dir.y * 0.5 + 0.5); // y: -1 (down) to 1 (up)
                return lerp(_BottomColor, _TopColor, t);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}