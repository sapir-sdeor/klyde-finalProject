Shader "Custom/ShaderMask"
{
   subShader
   {
     Tags{ "Queue" = "Transparent+1"}
       
     Pass{
         Blend Zero One
     }
   }
}
