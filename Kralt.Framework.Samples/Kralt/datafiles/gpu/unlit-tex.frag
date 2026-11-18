#version 330 core

out vec4 FragColor;

in vec2 uv;

uniform sampler2D texture0;

void main()
{
	float interp = texture(texture0, uv).r;

	vec4 c1 = vec4(1.0, 1.0, 1.0, 1.0);
	vec4 c2 = vec4(0.7, 0.7, 0.7, 1.0);

	FragColor = mix(c1, c2, interp);
}