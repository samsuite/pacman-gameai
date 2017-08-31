Shader "Ghost"
{
	Properties
	{
		_MainTex ("Base Texture", 2D) = "white" {}
		_BaseTint ("Base Tint", Color) = (1, 1, 1, 1)
		_Eyes ("Eyes Texture", 2D) = "white" {}
		_EyesTint ("Eyes Tint", Color) = (1, 1, 1, 1)
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _Eyes;

			float4 _BaseTint;
			float4 _EyesTint;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 eyecol = tex2D(_Eyes, i.uv) * _EyesTint;
				if (eyecol.a > 0) {
					return eyecol;
				}

				fixed4 basecol = tex2D(_MainTex, i.uv) * _BaseTint;
				if (basecol.a > 0) {
					return basecol;
				}

				discard;
				return 1;
			}
			ENDCG
		}
	}
}
