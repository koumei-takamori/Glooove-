//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/19
// <file>			VolumetricLightShader.h
// <概要>		　　ライトの光の筋のシェーダ
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
Shader "Custom/VolumetricLightShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,0.5)
        _FadeScaleY("_FadeScaleY" , Float) = 1
    }
    SubShader
    {
        Tags{"Queue" = "Transparent" "RenderType" = "Transparent"}
        LOD 100
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _FadeScaleY;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 localPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.localPos = v.vertex.xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float normalizedY = saturate(i.localPos.z * _FadeScaleY);
                float fade = normalizedY;
                return fixed4(_Color.rgb, _Color.a * fade);
            }
            ENDCG
        }

    }
}
