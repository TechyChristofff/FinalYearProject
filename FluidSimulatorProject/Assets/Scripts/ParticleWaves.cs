using UnityEngine;
 
public class ParticleWaves : MonoBehaviour {
 
        public ParticleSystem particlesSystem;
        private ParticleSystem.Particle[] particlesArray;
        public Gradient colorGradient;
        public int seaResolution = 50;
        public float spacing = 0.5f;
        public float noiseScale = 0.1f;
        public float heightScale = 1f;
        private float perlinNoiseAnimX = 0.01f;
        private float perlinNoiseAnimY = 0.01f;
        void Start() {
                particlesArray = new ParticleSystem.Particle[seaResolution * seaResolution];
                
                particlesSystem.maxParticles = seaResolution * seaResolution;
                particlesSystem.Emit(seaResolution * seaResolution);
                particlesSystem.GetParticles(particlesArray);
        }
        void Update() {
                for(int i = 0; i < seaResolution; i++) {
                        for(int j = 0; j < seaResolution; j++) {
                                float zPos = Mathf.PerlinNoise(i * noiseScale + perlinNoiseAnimX, j * noiseScale + perlinNoiseAnimY);
                                particlesArray[i * seaResolution + j].color = colorGradient.Evaluate(zPos);
                                particlesArray[i * seaResolution + j].position = new Vector3(i * spacing, zPos  * heightScale, j * spacing);
                        }
                }
 
                perlinNoiseAnimX += 0.01f;
                perlinNoiseAnimY += 0.01f;
 
                particlesSystem.SetParticles(particlesArray, particlesArray.Length);
        }
 
}