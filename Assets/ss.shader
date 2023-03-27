
Shader "ShaderMan/Checkerboard"
{
	Properties{
	
	}
	SubShader
	{
        Tags
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
        }
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct VertexInput {
                float4 vertex : POSITION;
                float2 uv:TEXCOORD0;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                //VertexInput
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv:TEXCOORD0;
                //VertexOutput
            };
            
            
            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;
                o.pos = UnityObjectToClipPos (v.vertex);
                o.uv = v.uv;
                //VertexFactory
                return o;
            }
            
            float checkerboard(float2 coord, float size){
                float2 pos = floor(coord/size); 
                return fmod(pos.x+pos.y,2.0);
            }
            fixed4 frag(VertexOutput vertex_output) : SV_Target
            {
                vertex_output.uv += _Time.y*0.1;
                float size = 0.1;
                float c = checkerboard(vertex_output.uv,size);
                float3 col1 = float3(1.0, 0.7176471, 0.4470588);
                float3 col2 = float3(0.4716981, 0.2955924, 0.1268245);
                float3 col = lerp(col1, col2, c);
                return float4(col,1.0);
            }
            ENDCG
        }
    }
}
