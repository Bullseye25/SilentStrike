Shader "Hidden/ThermalNightVisionImageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile TYPE_LUMINANCE TYPE_COLOR
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 scrPos : TEXCOORD0;
				float2 uv : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			float _Threshold;
			half4 _TypeColorValue;
			half _HotIntensity;
			half _ColdIntensity;
			half4 _ColdColor;
			half4 _MidColor;
			half4 _HotColor;

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.scrPos = ComputeScreenPos(o.pos);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 c = 0;
				float2 uv = i.scrPos.xy/i.scrPos.w;
				float4 mainC = tex2D(_MainTex, uv); 
				float luminance = 0;
#if defined(TYPE_LUMINANCE)
				luminance = 0.299 * mainC.r + 0.587 * mainC.g + 0.114 * mainC.b;
#endif
#if defined(TYPE_COLOR)
				luminance = dot(mainC.rgb, _TypeColorValue.rgb);
#endif
				c = (luminance < _Threshold) ? lerp(_ColdColor, _MidColor, luminance * _ColdIntensity ) : lerp(_MidColor, _HotColor, (luminance - 0.5) * _HotIntensity);
				c.rgb *= 0.1 + 0.25 + 0.75 * pow( 16.0 * uv.x * uv.y * (1.0 - uv.x) * (1.0 - uv.y), 0.15 );
				return c;
			}

			ENDCG
		}
	}
}
