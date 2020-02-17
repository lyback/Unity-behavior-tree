// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Jee/Normal/AlphaRG" {
    Properties {
  	    _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Maintex", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            //Tags {
            //    "LightMode"="ForwardBase"
            //}
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
    
            #include "UnityCG.cginc"


            #pragma target 2.0
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
        
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o ;
                o.uv0 = TRANSFORM_TEX(v.texcoord0, _MainTex);
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
     
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {

                float4 _Maintex_var = tex2D(_MainTex,i.uv0);
                float3 finalColor = (_Maintex_var.r*i.vertexColor.rgb);     
                fixed4 finalRGBA = fixed4(finalColor *i.vertexColor.rgb * _Color.rgb,(_Maintex_var.g*i.vertexColor.a * _Color.a));            
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
