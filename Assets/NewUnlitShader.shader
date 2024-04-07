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
                // ���� �߽��� �������� �� UV ��ǥ�� �Ÿ� ���
                float dist = distance(i.uv, float2(0.5, 0.5));
                // �ƿ����� ���ǿ� �´��� üũ
                bool inOutline = dist > (0.5 - _Outline) && dist < 0.5;
                // ���� ��� (radians)
                float angle = atan2(i.uv.y - 0.5, i.uv.x - 0.5) + 3.14159; // -PI ~ PI
                // Ư�� ���� ���� �ִ��� Ȯ��
                bool inQuarter = angle > 0 && angle < 1.5708; // 0 ~ 90��

                // �ƿ����� ���ǰ� ���� ������ �����ϸ� �ƿ����� ���� ����
                if (inOutline && inQuarter)
                {
                    return _OutlineColor;
                }
                // �� �� ������ ���� ó��
                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
