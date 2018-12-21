struct VsOutput
{
	float4 Position : SV_POSITION;
	float4 Color    : COLOR0;
};

VsOutput main(float4 position : POSITION0, float4 color : COLOR0)
{
	VsOutput output;
	output.Position = position;
	output.Color = color;
	return output;
}
