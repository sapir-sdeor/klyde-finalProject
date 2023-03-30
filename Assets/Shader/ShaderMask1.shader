Shader "Custom/ShaderMask1"
{
   subShader
   {
     Tags{ "Queue" = "Transparent+2"}
       
     Pass{
         Blend Zero One
     }
   }
}