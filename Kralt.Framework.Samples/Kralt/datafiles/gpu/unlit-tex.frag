#version 330 core

out vec4 FragColor;

in vec3 color;
in vec4 uv0;
in vec4 uv1;

uniform sampler2D texture0;

void main()
{
	float interp = texture(texture0, uv0.xy).r;

	vec4 c1 = vec4(1.0, 1.0, 1.0, 1.0);
	vec4 c2 = vec4(0.7, 0.7, 0.7, 1.0);

	FragColor = mix(c1, c2, interp);
}