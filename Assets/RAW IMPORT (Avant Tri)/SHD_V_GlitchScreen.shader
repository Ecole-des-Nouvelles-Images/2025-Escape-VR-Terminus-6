Shader "Elk/SHD_V_GlitchScreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VideoTex ("Video Texture", 2D) = "black" {}
        _ClipColor ("Clip Color", Color) = (0, 0, 0, 1)
        _EmissionColor ("Emission Color", Color) = (0, 0, 0, 1)
        _EmissionPower ("Emission Power", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            sampler2D _VideoTex;
            float4 _ClipColor;
            float4 _EmissionColor;
            float _EmissionPower;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the main texture
                fixed4 mainColor = tex2D(_MainTex, i.uv);

                // Sample the video texture
                fixed4 videoColor = tex2D(_VideoTex, i.uv);

                // Subtract the black color from the video texture
                videoColor = saturate(videoColor - _ClipColor);

                // Combine the colors
                fixed4 finalColor = mainColor + videoColor;

                // Add emission
                finalColor += _EmissionColor * _EmissionPower;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
