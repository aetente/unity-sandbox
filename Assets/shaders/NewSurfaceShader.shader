Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _ScreenTex ("screen", 2D) = "" {}
        _InvFade ("Soft Factor", Range(0.01,3.0)) = 1.0
        _fog_amount ("fog amount", Range(0,1)) = 0.0
        _fog_intensity ("fog intensity", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _ScreenTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
            float eyeDepth;
        };

        fixed4 _Color;

        half _FoamStrength;
        fixed4 _SpecularDepth, _SpecularEdge;
        uniform sampler2D _CameraDepthTexture;
        float4 _CameraDepthTexture_TexelSize;

        float _InvFade;
        float _fog_amount;
        float _fog_intensity;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            COMPUTE_EYEDEPTH(o.eyeDepth);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            
            // vec2 base_screen_uv = SCREEN_UV;
            // base_screen_uv.y += sin(TIME)/2.;
            // vec4 original = texture(SCREEN_TEXTURE, base_screen_uv);
            


            // vec4 depth_texture = texture(DEPTH_TEXTURE, base_screen_uv);
            // float depth = texture(DEPTH_TEXTURE, base_screen_uv).x;
            // vec3 ndc = vec3(base_screen_uv * 2.0 - 1.0, depth);
            
            // vec4 view = INV_PROJECTION_MATRIX* vec4(ndc, 1.0);
            // view.xyz /= view.w;


            float4 base_screen_uv = UNITY_PROJ_COORD(IN.screenPos);
            float4 depth_texture = tex2Dproj(_CameraDepthTexture, base_screen_uv);
            float depth = depth_texture.r;

            
	        float4 original = tex2D(_ScreenTex, base_screen_uv);

            float4 ndc = float4(base_screen_uv.xyz * 2.0 - 1.0, depth);

            // float4 view = float4(UnityObjectToViewPos(ndc), 1.0);
            // view.xyz /= view.w;
            float3 view = UnityObjectToViewPos(ndc);
            float linear_depth = -view.z;
            
	        float fog = linear_depth * _fog_amount;
            
	        float4 fog_color = tex2D(_ScreenTex, float2(fog, 0.0));

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            
            if (linear_depth > 1.0)
                o.Albedo =  lerp(original.rgb, fog_color.rgb, fog_color.a * _fog_intensity);
            else
                o.Albedo = fog_color.rgb;
            o.Albedo = original.rgb;
            // Metallic and smoothness come from slider variables
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
