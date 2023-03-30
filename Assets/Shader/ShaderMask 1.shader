Shader "Custom/ShaderMask"
{
   subShader
   {
     Tags{ "Queue" = "Transparent+2"}
       
     Pass{
         Blend Zero One
     }
   }
}
