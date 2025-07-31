#version 430

layout(std430, binding = 5) buffer ssbo {
uint map[];
};

in vec3 vertexPos;
in vec2 fragTexCoord;
in vec4 fragColor;

uniform sampler2D texture0;
uniform int width;
uniform int height;
uniform vec3 countries[128];

out vec4 finalColor;

void main() {
int x = int(fragTexCoord.x * float(width));
int y = int(fragTexCoord.y * float(height));

int idx = y * width + x;

uint countryId = map[idx];
vec3 baseColor = countries[countryId];

if(countryId == 0){
    finalColor = vec4(0.0,0.0,0.0,0.0);    
    return;
}


finalColor = vec4(countries[countryId], 0.5);
}