#version 330 core

layout(location = 0) in vec3 POSITION;
layout(location = 1) in vec3 COLOR;
layout(location = 2) in vec4 TEXCOORD0;
layout(location = 3) in vec4 TEXCOORD1;

out vec4 uv0;
out vec4 uv1;

uniform mat4 TR_MESH;
uniform mat4 TR_VIEW;
uniform mat4 TR_PROJ;

void main()
{
	uv0 = TEXCOORD0;
	uv1 = TEXCOORD1;
	gl_Position = vec4(POSITION, 1.0) * TR_MESH * TR_VIEW * TR_PROJ;
}