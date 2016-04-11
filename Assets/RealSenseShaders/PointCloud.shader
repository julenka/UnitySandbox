Shader "Hidden/PointCloud" {
    Properties{
        _PointSize("PointSize", Float) = 1
    }

    SubShader{
        Pass{
        LOD 200

        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct VertexInput {
                float4 v : POSITION;
                float4 color: COLOR;
                float2 uv : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float size : PSIZE;
                float4 col : COLOR;
            };

            float _PointSize;
            sampler2D _MainTex;

            VertexOutput vert(VertexInput v) {

                float4 tex = tex2Dlod(_MainTex, float4(v.uv, 0, 0));
                v.v.y += tex.r / 0.1f;

                VertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP, v.v);
                o.size = _PointSize;
                o.col = v.color;

                return o;
            }

            float4 frag(VertexOutput o) : COLOR{
                return o.col;
            }

        ENDCG
        }
    }

}
