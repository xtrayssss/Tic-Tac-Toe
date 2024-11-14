Shader "Custom/AnimatedGradient"
{
    Properties
    {
        _Speed ("Animation Speed", Float) = 1.0
        _GradientAngle ("Gradient Angle", Range(0, 360)) = 267
        _ColorA ("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (0,0,0,1)
        _Scale ("Gradient Scale", Float) = 4.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

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

            float _Speed;
            float _GradientAngle;
            float4 _ColorA;
            float4 _ColorB;
            float _Scale;

            // Функция сглаживания
            float smoothStep(float edge0, float edge1, float x) 
            {
                x = clamp((x - edge0) / (edge1 - edge0), 0.0, 1.0);
                return x * x * (3 - 2 * x);
            }

            // Функция для создания плавного повторяющегося значения
            float smoothRepeat(float t)
            {
                float cycle = fmod(t, 2.0);
                if(cycle > 1.0)
                    cycle = 2.0 - cycle;
                return cycle;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float angleRad = radians(_GradientAngle);
                float2 dir = float2(cos(angleRad), sin(angleRad));
                
                // Более плавная анимация
                float movement = sin(_Time.y * _Speed) * 0.5;
                
                // Масштабируем UV координаты
                float2 scaledUV = (i.uv - 0.5) * _Scale;
                
                // Вычисляем позицию в градиенте
                float gradientPos = dot(scaledUV, dir) + movement;
                
                // Создаем плавное повторение градиента
                gradientPos = smoothRepeat(gradientPos);
                
                // Сглаживаем переход
                gradientPos = smoothStep(0.0, 1.0, gradientPos);
                
                // Интерполируем между цветами
                fixed4 col = lerp(_ColorA, _ColorB, gradientPos);
                return col;
            }
            ENDCG
        }
    }
}