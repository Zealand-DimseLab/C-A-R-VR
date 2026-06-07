Shader "Custom/Indirect/TreeTrunk_Advanced" {
    Properties {
        _MainTex ("Bark Texture", 2D) = "white" {}
        _Color ("Main Tint", Color) = (1,1,1,1)

        [Header(Colors)]
        _BaseColor ("Root Tint", Color) = (0.2, 0.15, 0.1, 1)
        _TopColor ("Crown Tint", Color) = (0.4, 0.3, 0.2, 1)
        _ColorVariationScale ("Variation Scale", Float) = 0.05

        [Header(Wind)]
        _WindSpeed ("Wind Speed", Float) = 0.8
        _WindStrength ("Wind Strength", Float) = 0.05
        _WindDirection ("Wind Direction", Vector) = (1, 0, 0.5, 0)
    }
    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }

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
        float4 _Color, _BaseColor, _TopColor, _WindDirection;
        float _WindSpeed, _WindStrength, _ColorVariationScale;

        float simpleNoise(float2 p) {
            return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
        }
        ENDHLSL

        // --- 1. FORWARD LIT PASS (This makes the trunk visible) ---
        Pass {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                uint instanceID : SV_InstanceID;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                half3 variationColor : COLOR;
                float4 shadowCoord : TEXCOORD3;
            };

            Varyings vert (Attributes v) {
                Varyings o = (Varyings)0;
                
                TreeData data = _InstanceBuffer[v.instanceID];
                float4x4 instanceMatrix = GetInstanceMatrix(data);

                // Wind
                float h = saturate(v.positionOS.y * 0.1); 
                float wave = sin(_Time.y * _WindSpeed + (data.Position.x * 0.5));
                v.positionOS.xz += _WindDirection.xz * (wave * _WindStrength * h);

                float3 positionWS = mul(instanceMatrix, float4(v.positionOS.xyz, 1.0)).xyz;
                o.positionCS = TransformWorldToHClip(positionWS);
                o.uv = v.uv;
                o.normalWS = TransformObjectToWorldNormal(mul((float3x3)instanceMatrix, v.normalOS));
                
                // Color variation based on world position
                float noise = simpleNoise(data.Position.xz * _ColorVariationScale);
                o.variationColor = lerp(_BaseColor.rgb, _TopColor.rgb, noise);
                
                o.shadowCoord = TransformWorldToShadowCoord(positionWS);
                return o;
            }

            half4 frag (Varyings i) : SV_Target {
                half4 tex = tex2D(_MainTex, i.uv) * _Color;
                
                Light mainLight = GetMainLight(i.shadowCoord);
                float ndl = saturate(dot(i.normalWS, mainLight.direction));
                half3 ambient = SampleSH(i.normalWS);
                
                half3 lightColor = mainLight.color * (ndl * mainLight.shadowAttenuation);
                half3 finalColor = tex.rgb * i.variationColor * (lightColor + ambient);
                
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }

        // --- 2. SHADOW CASTER PASS (This makes the shadows) ---
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
                float3 normalOS : NORMAL;
                uint instanceID : SV_InstanceID;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert (Attributes v) {
                Varyings o = (Varyings)0;
                TreeData data = _InstanceBuffer[v.instanceID];
                float4x4 instanceMatrix = GetInstanceMatrix(data);
                
                float h = saturate(v.positionOS.y * 0.1);
                float wave = sin(_Time.y * _WindSpeed + (data.Position.x * 0.5));
                v.positionOS.xz += _WindDirection.xz * (wave * _WindStrength * h);

                float3 positionWS = mul(instanceMatrix, float4(v.positionOS.xyz, 1.0)).xyz;
                float3 normalWS = TransformObjectToWorldNormal(mul((float3x3)instanceMatrix, v.normalOS));

                o.positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _MainLightPosition.xyz));
                return o;
            }

            half4 frag () : SV_Target {
                return 0;
            }
            ENDHLSL
        }
    }
}
