Shader "Custom/Stencil"
{
    Properties
    {
        [IntRange] _StencilID ("Stencil ID", Range(0,255)) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
            "RenderPipeline" = "UniversalPipeline"
        }
        Cull Off

        Pass
        {
            Name "StencilWrite"
            Tags { "LightMode" = "UniversalForward" }
            Cull Off
            
            Blend Zero One
            ZWrite Off

            Stencil
            {
                Ref [_StencilID]
                Comp Always
                Pass Replace
                Fail Keep
            }
        }

        Pass
        {
            Name "StencilRender"
            Tags { "LightMode" = "UniversalForward" }
            Cull Off
            
            Stencil
            {
                Ref [_StencilID]
                Comp Equal
                Pass Keep
                Fail Keep
                ZFail Keep
            }
        }
    }
}
