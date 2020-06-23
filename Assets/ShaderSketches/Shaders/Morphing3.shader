﻿Shader "ShaderSketches/Morphing3"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white"{}
    }

    CGINCLUDE
    #include "UnityCG.cginc"
    #include "Common.cginc"

    #define PI 3.14159265359

    float2 rotate(float2 st, float angle)
    {
        float2x2 mat = float2x2(cos(angle), -sin(angle),
                                sin(angle), cos(angle));

        st -= 0.5;
        st = mul(mat, st);
        st += 0.5;

        return st;
    }

    float2 scale(float2 st, float2 scale)
    {
        st -= 0.5;
        st *= scale;
        st += 0.5;

        return st;
    }

    float2 hex(float2 st)
    {
        st -= 0.5;
        st = abs(st);
        float2 r = 0.005;

        return float2(max(st.x - r.y, max(st.x + st.y * 0.57735, st.y * 1.1547) - r.x), 0.4);
    }

    float2 star(float2 st)
    {
        st -= 0.5;
        st *= 0.9;

        float a = atan2(st.y, st.x) + _Time.y * 0.3;
        float l = pow(length(st), 0.6);
        float d = l - 0.5 + cos(a * 5.0) * 0.08;

        return float2(d, 0);
    }
    
    float2 heart(float2 st)
    {
        st = scale(st, 1 / 1.8);
        st = (st - float2(0.5, 0.38)) * float2(2.1, 2.8);

        float a = st.x;
        float b = st.y - sqrt(abs(st.x));

        return float2(a * a + b * b, 0.2);
    }

    float2 circle(float2 st)
    {
        return float2(length(0.5 - st), 0.2);
    }

    float tone(float2 st, float size)
    {
        float c = length(0.5 - st);
        return step(c, 0.45) - step(c, size * 0.45);
    }

    float lerp_shape(float2 from, float2 to, float a)
    {
        return step(lerp(from.x, to.x, a), lerp(from.y, to.y, a));
    }

    float morphing(float2 st)
    {

        float t = _Time.y * 3;
        int it = floor(t) % 4;
        float a = smoothstep(0, 0.6, frac(t));

        switch (it)
        {
            case 0:
                return lerp_shape(heart(st), circle(st), a);
            case 1:
                return lerp_shape(circle(st), hex(st), a);
            case 2:
                return lerp_shape(hex(st), star(st), a);
            case 3:
                return lerp_shape(star(st), heart(st), a);
        }

        return 0;
    }

    float halftone(float2 st)
    {
        float n = 13;
        float angle = -_Time.y * PI * 0.15;

        st = rotate(st, angle);

        float2 ist = floor(st * n);
        float2 fst = frac(st * n);

        st = rotate((ist + 0.5) / n, -angle);
        st = scale(st, 1 / 3.5);

        return morphing(fst) * morphing(st);
    }

    float4 frag(v2f_img i) : SV_Target
    {
        i.uv = screen_aspect(i.uv);
        i.uv = scale(i.uv, 1.0 + 15 * pow(length(i.uv - 0.5), 2));

        return lerp(float4(0.8, 0.8, 0.16, 1),
                    float4(0.8, 0.16, 0.58, 1),
                    halftone(i.uv));
    }

    ENDCG

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            ENDCG
        }
    }
}
