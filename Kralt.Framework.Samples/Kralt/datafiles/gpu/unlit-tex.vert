#version 330 core

layout(location = 0) in vec3 POSITION;
layout(location = 2) in vec2 TEXCOORD0;

out vec2 uv;

uniform mat4 TR_MESH;
uniform mat4 TR_VIEW;
uniform mat4 TR_PROJ;

void main()
{
	uv = TEXCOORD0;
	gl_Position = vec4(POSITION, 1.0) * TR_MESH * TR_VIEW * TR_PROJ;
}