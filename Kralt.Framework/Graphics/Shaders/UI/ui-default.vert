#version 330 core

layout(location = 0) in vec2 POSITION;
layout(location = 1) in vec4 COLOR;

out vec4 color;

uniform vec2 VIEWPORT;

void main()
{
	color = COLOR;
	gl_Position = vec4(POSITION.x / VIEWPORT.x, POSITION.y / VIEWPORT.y, 0.0, 0.0);
}