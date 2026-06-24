#version 330 core
layout (location = 0) in vec3 aPos; 
layout (location = 1) in vec2 aTexcord;
uniform mat4 transform;

out vec2 texCord; 

void main()
{
    texCord = aTexcord;
    
    gl_Position = vec4(aPos, 1.0) * transform; 
}