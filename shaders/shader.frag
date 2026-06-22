#version 330 core
out vec4 FragColor;

// passed from vertex shader
in vec2 texCord;
uniform sampler2D texture0;
void main()
{
  
    FragColor = texture(texture0,texCord);
} 
