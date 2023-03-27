
    Shader "ShaderMan/ slanted squares"
	{
	Properties{
	
	}
	SubShader
	{
	Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
	Pass
	{
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"
			
    

    float4 vec4(float x,float y,float z,float w){return float4(x,y,z,w);}
    float4 vec4(float x){return float4(x,x,x,x);}
    float4 vec4(float2 x,float2 y){return float4(float2(x.x,x.y),float2(y.x,y.y));}
    float4 vec4(float3 x,float y){return float4(float3(x.x,x.y,x.z),y);}


    float3 vec3(float x,float y,float z){return float3(x,y,z);}
    float3 vec3(float x){return float3(x,x,x);}
    float3 vec3(float2 x,float y){return float3(float2(x.x,x.y),y);}

    float2 vec2(float x,float y){return float2(x,y);}
    float2 vec2(float x){return float2(x,x);}

    float vec(float x){return float(x);}
    
    

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
    
    float fold(float x) {
    return 2.0 * abs(0.5 - frac(x));
}


    
    
	fixed4 frag(VertexOutput vertex_output) : SV_Target
	{
	
    float time = frac(_Time.y / 2.);
    // Scales coords so that the diagonals are all dist 1 from center
    float scale = length(1);
    float2 uv = (vertex_output.uv / scale
	- (1 / scale / 2.)) * 15.;
    
    // diagonal grid
    float2 slantUV = vec2(uv.x + uv.y, uv.x - uv.y);
    float2 slantRnd = floor(slantUV);
    slantUV = frac(slantUV + 0.5) - 0.5;
    
    // spiral
    float r = length(slantRnd);
    float angle = atan2( slantRnd.x,slantRnd.y) / 2.0 / 3.14159265358979;
    float spiral = fold(r * 0.15 + angle + time - cos(time * 3.1415927));

    float thres = 
        min(abs(slantUV.x), abs(slantUV.y));
        float zig = step(1., 1.5 * frac(time * 1. + abs(floor(0.25 + slantRnd.y - slantRnd.x)) / 6.));

    float3 col = lerp(
        lerp(vec3(0.3, 0.1, 0.1), vec3(0.5, 0.2, 0.0), zig),
        lerp(vec3(0.1, 0.5, 0.8), vec3(0.0, 0.8, 1.0), zig),
        
        smoothstep(
            0.7, 0.8,
            spiral + thres
        )
    );

    // Output to screen
    return vec4(col,1.0);

	}
	ENDCG
	}
  }
  }
