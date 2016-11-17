// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/CuttingPlane"
{
    Properties
    {
        _Tint("Tint", Color) = (1,1,1,1)
        _InnerRadius("Inner Radius", float) = 0
        _OuterRadius("Outer Radius", float) = 1
        _Center("Center", Vector) = (0,0,0)
    }
        SubShader
    {
        Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Pass
        {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                float4 _Tint;
                float _InnerRadius;
                float _OuterRadius;
                float4 _Center;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    // Vertex as projected onto display (model-view-projection)
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    // Vertex in world coords
                    float3 vertex_world : TEXCOORD1;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                    o.vertex_world = mul(unity_ObjectToWorld, v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    float r = length(i.vertex_world - _Center.xyz);
                    
                    if (r < _InnerRadius || r > _OuterRadius) {
                        return 0;
                    }

                    return float4(i.uv, 1, 1) * _Tint;
                }
            ENDCG
    }
    }
}
