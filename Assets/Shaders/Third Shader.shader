Shader "Custom/Third Shader" {
	SubShader{
	Pass {
		/*The whole snippet is written between CGPROGRAM
		 and ENDCG keywords.At the start compilation directives are given as #pragma statements:
		#pragma vertex name tells that the code contains a vertex program in the given function (vert here).
		#pragma fragment name tells that the code contains a fragment program in the given function (frag here).*/
		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		/*The UnityCG.cginc file contains commonly used declarations and functions so that the shaders can be kept smaller.
		Here we’ll use appdata_base structure from that file.
		We could just define them directly in the shader and not include the file of course.*/
		#include "UnityCG.cginc"
		/*Next we define a “vertex to fragment” structure(here named v2f) 
		- what information is passed from the vertex to the fragment program.
		We pass the position and color parameters.
		The color will be computed in the vertex program and just output in the fragment program.*/
		struct v2f {
			float4 pos : SV_POSITION;
			fixed3 color : COLOR0;
		};

		v2f vert(appdata_base v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.color = v.normal * 0.5 + 0.5;
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			return fixed4(i.color, 1);
		}
		ENDCG

	}
	}
}
