Shader "Unlit/OneSidedWhite"
{
    Properties {
        _Color("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags { "Queue"="Overlay" }
        Cull Back
        ZWrite Off
        Lighting Off
        Pass {
            Color[_Color]
        }
    }
}
