#version 100
precision mediump float;


varying vec2 fragTexCoord;
varying vec4 fragColor;

uniform sampler2D texture0;

void main() {
    vec4 baseColor = texture2D(texture0, fragTexCoord);

    if(baseColor.a > 0){
        baseColor.a = 0.5;
    }

    gl_FragColor  = baseColor;
}