#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 view_projection;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 ShadowPS(VertexShaderOutput input) : COLOR
{
	float shadowRadius = 0.30;	
	float2 shadowCenter = input.TextureCoordinates - float2(0.52, 0.52);

	float shadow = 0.75 + 0.25 * smoothstep(shadowRadius - (shadowRadius * 0.5),
                              shadowRadius + (shadowRadius * 0.5),
                              dot(shadowCenter, shadowCenter) * 5.0);
	
	return float4(0, 0, 0, 1 - shadow);
}

float4 BallPS(VertexShaderOutput input) : COLOR
{
	float radius = 0.25;
	float2 center = input.TextureCoordinates - float2(0.5, 0.5);
	float alpha = 1. - smoothstep(radius - (radius * 0.02),
                                  radius + (radius * 0.02),
                                  dot(center, center) * 4.0);
	float ballShine = 0.25 + 0.75 * (1. - distance(input.TextureCoordinates, float2(0.25, 0.25)));
		
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float4 ballColor = float4(ballShine, ballShine, ballShine, 1.);
	float4 finalColor = (ballColor * (1 - color[3])) + ((color * 0.25 + ballColor[0] * 0.75) * color[3]);
	finalColor[3] = 1.;
	return finalColor * alpha;
}
technique SpriteDrawing
{
	pass P0
	{        
		PixelShader = compile PS_SHADERMODEL ShadowPS();
	}
	pass P1
	{
		PixelShader = compile PS_SHADERMODEL BallPS();
	}
};