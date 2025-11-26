#include "UnityCG.cginc"
#include "UnityLightingCommon.cginc"

static float4 GetLightColor()
{
    float4 lightColor = _LightColor0;
    lightColor = lightColor == float4(0, 0, 0, 0) ? float4(1, .9f, .8f, 1) : lightColor;
    return lightColor;
}

static float4 GetLightDir()
{
    float4 lightDir = _WorldSpaceLightPos0;
    lightDir = lightDir == float4(0, 0, 0, 0) ? float4(0.00f, 0.87f, 0.50f, 0) : lightDir;
    return lightDir;
}
static float4 GetCameraDir()
{
    float3 cameraDir = _WorldSpaceCameraPos;
    return float4(cameraDir, 0);
}

static float GetAmbient()
{
    float4 ambientColor = (float4(1, 1, 1, 1) - GetLightColor());
    return ambientColor;
}

static float GetDiffuse(float4 normal)
{
    float diffuse = saturate(dot(GetLightDir(), normal));
    diffuse = max(diffuse,0);
    return diffuse;
}

static float GetSpecular(float4 worldPos, float4 normal, float reflectivenessExponent)
{
    float3 reflectionDir = normalize(GetCameraDir() - worldPos);
    float4 reflection = reflect(float4(-reflectionDir, 0), normalize(normal));
    float specular = dot(reflection, GetLightDir());
    specular = pow(specular, reflectivenessExponent);
    //specular = saturate(specular);
    float4 specular4 = saturate(float4(specular.xxx, 0) * GetLightColor());
    return specular4.x;
}

static float4 ApplyCellShading(float4 diffusePlusSpecular, int cellShadeLoops)
{
    float result = ceil(diffusePlusSpecular * cellShadeLoops + pow(cellShadeLoops, -1)) / cellShadeLoops;
    return result;
}

static float4 GetLighting(float4 col, float4 normal, float4 worldPos, float reflectivenessExponent)
{
    // Lighting
    float4 ambientColor = GetAmbient();
    // Diffuse
    float diffuse = GetDiffuse(normal);
    // Specular
    float specular = GetSpecular(worldPos, normal, reflectivenessExponent);
    diffuse += specular;
				
    col *= float4(diffuse.xxx, 0) * GetLightColor() + ambientColor;
    
    return col;
}

static float4 GetLightingCellshade(float4 col, float4 normal, float4 worldPos, float reflectivenessExponent, int cellShadeLoops)
{
    // Lighting
    float4 ambientColor = GetAmbient();
    // Diffuse
    float diffuse = GetDiffuse(normal);
    // Specular
    float specular = GetSpecular(worldPos, normal, reflectivenessExponent);
    diffuse += specular;
    // Cell shade conversion
    diffuse = ApplyCellShading(diffuse, cellShadeLoops);
				
    col *= (diffuse + ambientColor) * GetLightColor();
    
    return col;
}
