#version 430

layout(std430, binding = 5) buffer ssbo {
uint data_SSBO[];
};

in vec3 vertexPos;
in vec2 fragTexCoord;
in vec4 fragColor;

uniform sampler2D texture0;
uniform int width;
uniform int height;

out vec4 finalColor;

vec4 GetTerrainColor(uint type) {
if(type == 7) {
return vec4(1.0, 1.0, 1.0, 1.0);
}

if(type == 6) {
return vec4(0.4, 0.4, 0.4, 1.0);
}

if(type == 5) {
return vec4(0.7, 0.6, 0.0, 1.0);
}

if(type == 4) {
return vec4(0.0, 0.6, 0.0, 1.0);
}

if(type == 3) {
return vec4(0.0, 1.0, 0.0, 1.0);
}

if(type == 2) {
return vec4(1.0, 1.0, 0.0, 1.0);
}

if(type == 1) {
return vec4(0.0, 0.0, 1.0, 1.0);
}

return vec4(1.0, 0.0, 0.0, 0.0);
}

void main() {
int x = int(fragTexCoord.x * float(width));
int y = int(fragTexCoord.y * float(height));

int idx = y * width + x;

uint tileType = data_SSBO[idx];

finalColor = GetTerrainColor(tileType);
}