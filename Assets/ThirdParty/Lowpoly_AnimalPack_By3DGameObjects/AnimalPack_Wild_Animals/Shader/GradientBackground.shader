Shader "Custom/URPGradientBackgroundLit"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 1, 1, 1)
        _Color2 ("Color 2", Color) = (1, 0, 0, 1)

        [Enum(Linear,0,Radial,1)]
        _MaskMode ("Mask Mode", Float) = 0

        _MaskStrength ("Mask Strength", Range(0.1, 5)) = 1

        // 0 = çok yumuşak, 1 = neredeyse sert cut
        _EdgeSoftness ("Edge Softness", Range(0, 1)) = 0.0

        // Maske çapı / yarıçap kontrolü
        _MaskSize ("Mask Size", Range(0.1, 2.0)) = 1.0

        _Offset  ("UV Offset (X,Y)", Vector) = (0,0,0,0)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalRenderPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Cull Back
            ZWrite On
            ZTest LEqual
            Blend Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Gölge / ışık varyantları
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS  : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float2 uv          : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color1;
                float4 _Color2;
                float  _MaskMode;
                float  _MaskStrength;
                float  _EdgeSoftness;
                float  _MaskSize;
                float4 _Offset;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                float3 normalWS   = TransformObjectToWorldNormal(IN.normalOS);

                OUT.positionHCS = TransformWorldToHClip(positionWS);
                OUT.positionWS  = positionWS;
                OUT.normalWS    = normalize(normalWS);
                OUT.uv          = IN.uv;

                return OUT;
            }

            float4 frag (Varyings i) : SV_Target
            {
                // ------------ GRADIENT HESABI ------------

                // UV offset ile kaydır
                float2 uv = i.uv + _Offset.xy;

                // repeat olmasın
                uv = clamp(uv, 0.0, 1.0);

                float2 uvCentered = uv - 0.5;

                // Linear: 0–1 arası (alt -> üst)
                float tLinear = uv.y;

                // Radial: merkezden mesafe, MaskSize ile normalize
                float dist = length(uvCentered);
                float tRadial = dist / max(_MaskSize, 0.0001);

                // Maske tipi: 0 = linear, 1 = radial
                float maskMode = step(0.5, _MaskMode);
                float tRaw = lerp(tLinear, tRadial, maskMode);

                // MaskStrength ile "hızlandır / yavaşlat"
                tRaw = pow(saturate(tRaw), _MaskStrength);

                // --- Edge Softness davranışı ---
                // Center: geçişin ortası (0.5 civarında)
                float center = 0.5;

                // EdgeSoftness = 0  → geniş geçiş (yumuşak)
                // EdgeSoftness = 1  → çok dar geçiş (sert)
                float hardness = _EdgeSoftness;
                float width = lerp(0.5, 0.0001, hardness);

                // tRaw < center-width  → çoğunlukla Color1
                // tRaw > center+width  → çoğunlukla Color2
                float t = smoothstep(center - width, center + width, tRaw);

                // Renk karışımı
                float4 baseColor = lerp(_Color1, _Color2, t);
                baseColor.a = 1.0;

                // ------------ AYDINLATMA + GÖLGE ------------

                float3 normalWS = normalize(i.normalWS);

                // Ana ışık + gölge
                float4 shadowCoord = TransformWorldToShadowCoord(i.positionWS);
                Light mainLight = GetMainLight(shadowCoord);

                float NdotL = saturate(dot(normalWS, mainLight.direction));

                float3 direct = baseColor.rgb * mainLight.color * NdotL * mainLight.shadowAttenuation;

                // Basit ambient
                float3 ambient = baseColor.rgb * 0.2;

                float3 finalColor = direct + ambient;

                return float4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
