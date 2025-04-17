Shader "Custom/ProceduralLineShader"
{
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Pass {
            ZWrite Off
            ZTest Always
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float4 _Color;
            StructuredBuffer<float3> _LinePoints; // Contains start/end pairs

            struct appdata {
                uint vertexID : SV_VertexID;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                uint lineIndex = v.vertexID / 2;
                uint isEnd = v.vertexID % 2;

                float3 pos = _LinePoints[lineIndex * 2 + isEnd];
                o.pos = UnityObjectToClipPos(float4(pos, 1.0));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                return _Color;
            }
            ENDCG
        }
    }
}