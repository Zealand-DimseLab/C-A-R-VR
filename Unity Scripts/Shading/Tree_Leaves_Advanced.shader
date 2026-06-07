Shader "Custom/Indirect/TreeLeaves_Advanced" {
    Properties {
        _MainTex ("Leaf Texture", 2D) = "white" {}
        _CutOff ("Alpha Cutoff", Range(0, 1)) = 0.5
        
        [Header(Colors)]
        _BaseColor ("Bottom Color", Color) = (0.2, 0.3, 0.1, 1)
        _TopColor ("Top Color", Color) = (0.5, 0.7, 0.2, 1)
        _ColorVariationScale ("Variation Scale", Float) = 0.05
        
        [Header(Wind)]
        _WindSpeed ("Wind Speed", Float) = 1.0
        _WindStrength ("Wind Strength", Float) = 0.1
        _WindDirection ("Wind Direction", Vector) = (1, 0, 0.5, 0)
    }
    SubShader {
        Tags { "RenderType"="TransparentCutout" "RenderPipeline"="UniversalPipeline" "Queue"="AlphaTest" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        struct TreeData {
            float3 Position;
            float Yaw;
            float Scale;
        };

        StructuredBuffer<TreeData> _InstanceBuffer;

        float4x4 GetInstanceMatrix(TreeData data) {
            float s = data.Scale;
            float angle = data.Yaw * (3.14159265f / 180.0f);
            float cosY = cos(angle);
            float sinY = sin(angle);
            return float4x4(
                cosY * s,  0, sinY * s, data.Position.x,
                0,         s, 0,        data.Position.y,
                -sinY * s, 0, cosY * s, data.Position.z,
                0,         0, 0,        1
            );
        }

        sampler2D _MainTex;
        float _CutOff, _WindSpeed, _WindStrength, _ColorVariationScale;
        float4 _BaseColor, _TopColor, _WindDirection;

        float simpleNoise(float2 p) {
            return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
        }
        ENDHLSL

        // --- 1. FORWARD LIT PASS ---
        Pass {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            Cull Off 

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                uint instanceID : SV_InstanceID;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                half3 color : COLOR;
                float3 normalWS : TEXCOORD3;
                float4 shadowCoord : TEXCOORD5;
            };

            Varyings vert (Attributes v) {
                Varyings o = (Varyings)0;
                TreeData data = _InstanceBuffer[v.instanceID];
                float4x4 instanceMatrix = GetInstanceMatrix(data);
                
                // Wind logic
                float heightMask = saturate(length(v.positionOS.xyz)); 
                float wave = sin(_Time.y * _WindSpeed + (data.Position.x * 0.5) + (data.Position.z * 0.3));
                v.positionOS.xyz += _WindDirection.xyz * (wave * _WindStrength * heightMask);

                float3 positionWS = mul(instanceMatrix, float4(v.positionOS.xyz, 1.0)).xyz;
                o.positionCS = TransformWorldToHClip(positionWS);
                o.uv = v.uv;

                float noise = simpleNoise(data.Position.xz * _ColorVariationScale);
                o.color = lerp(_BaseColor.rgb, _TopColor.rgb, noise);
                o.normalWS = TransformObjectToWorldNormal(mul((float3x3)instanceMatrix, v.normalOS));
                o.shadowCoord = TransformWorldToShadowCoord(positionWS);

                return o;
            }

            half4 frag (Varyings i) : SV_Target {
                half4 tex = tex2D(_MainTex, i.uv);
                clip(tex.a - _CutOff);

                Light mainLight = GetMainLight(i.shadowCoord);
                float ndl = saturate(dot(i.normalWS, mainLight.direction));
                half3 ambient = SampleSH(i.normalWS);
                half3 lightColor = mainLight.color * (ndl * mainLight.shadowAttenuation);

                return half4(tex.rgb * i.color * (lightColor + ambient), 1.0);
            }
            ENDHLSL
        }

        // --- 2. SHADOW CASTER PASS ---
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                uint instanceID : SV_InstanceID;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert (Attributes v) {
                Varyings o = (Varyings)0;
                TreeData data = _InstanceBuffer[v.instanceID];
                float4x4 instanceMatrix = GetInstanceMatrix(data);

                // Wind logic (must match ForwardLit exactly!)
                float heightMask = saturate(length(v.positionOS.xyz)); 
                float wave = sin(_Time.y * _WindSpeed + (data.Position.x * 0.5) + (data.Position.z * 0.3));
                v.positionOS.xyz += _WindDirection.xyz * (wave * _WindStrength * heightMask);

                float3 positionWS = mul(instanceMatrix, float4(v.positionOS.xyz, 1.0)).xyz;
                float3 normalWS = TransformObjectToWorldNormal(mul((float3x3)instanceMatrix, v.normalOS));

                o.positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _MainLightPosition.xyz));
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target {
                half alpha = tex2D(_MainTex, i.uv).a;
                clip(alpha - _CutOff);
                return 0;
            }
            ENDHLSL
        }
    }
}
