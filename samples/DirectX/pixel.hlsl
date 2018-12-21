struct VsOutput
{
	float4 Position : SV_POSITION;
	float4 Color    : COLOR0;
};

float4 main(float4 pos : SV_POSITION, float4 color : COLOR) : SV_TARGET
{
	return color;
}
