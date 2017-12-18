// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

struct VsOutput
{
	float4 Position : SV_POSITION;
	float4 Color    : COLOR0;
};

float4 main(float4 pos : SV_POSITION, float4 color : COLOR) : SV_TARGET
{
	return color;
}