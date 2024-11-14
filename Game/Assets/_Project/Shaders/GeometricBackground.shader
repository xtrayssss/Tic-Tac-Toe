Shader "Custom/GeometricBackground"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Pattern Scale", Float) = 1.0
        _Opacity ("Pattern Opacity", Range(0,1)) = 0.03
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Scale;
            float _Opacity;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float2 rotate2D(float2 p, float angle)
            {
                float s = sin(angle);
                float c = cos(angle);
                return float2(p.x * c - p.y * s, p.x * s + p.y * c);
            }

            float gradientNoise(float2 uv, float angle)
            {
                uv = rotate2D(uv, angle);
                float2 i = floor(uv);
                float2 f = frac(uv);
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(dot(float2(1, 2), i), dot(float2(3, 4), i + float2(1, 0)), u.x),
                           lerp(dot(float2(5, 6), i + float2(0, 1)), dot(float2(7, 8), i + float2(1, 1)), u.x),
                           u.y) * 0.5 + 0.5;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * _Scale;
                
                // Multiple gradient layers
                float pattern = 0;
                pattern += gradientNoise(uv, 0.96); // 55 degrees
                pattern += gradientNoise(uv * 1.2, 5.08); // 291 degrees
                pattern += gradientNoise(uv * 0.8, 0.56); // 32 degrees
                pattern += gradientNoise(uv * 1.5, 5.44); // 312 degrees
                pattern += gradientNoise(uv * 0.9, 0.38); // 22 degrees
                pattern += gradientNoise(uv * 1.1, 1.40); // 80 degrees
                pattern += gradientNoise(uv * 1.3, 4.48); // 257 degrees
                pattern += gradientNoise(uv * 0.7, 0.82); // 47 degrees
                
                pattern = pattern / 8.0; // Normalize
                
                return fixed4(0.8, 0.8, 0.8, pattern * _Opacity);
            }
            ENDCG
        }
    }
}