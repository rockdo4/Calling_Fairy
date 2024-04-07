Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline Width", Range(0.0, 0.1)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

            fixed4 _Color;
            fixed4 _OutlineColor;
            float _Outline;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 원의 중심을 기준으로 한 UV 좌표의 거리 계산
                float dist = distance(i.uv, float2(0.5, 0.5));
                // 아웃라인 조건에 맞는지 체크
                bool inOutline = dist > (0.5 - _Outline) && dist < 0.5;
                // 각도 계산 (radians)
                float angle = atan2(i.uv.y - 0.5, i.uv.x - 0.5) + 3.14159; // -PI ~ PI
                // 특정 각도 내에 있는지 확인
                bool inQuarter = angle > 0 && angle < 1.5708; // 0 ~ 90도

                // 아웃라인 조건과 각도 조건을 만족하면 아웃라인 색상 적용
                if (inOutline && inQuarter)
                {
                    return _OutlineColor;
                }
                // 그 외 영역은 투명 처리
                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
