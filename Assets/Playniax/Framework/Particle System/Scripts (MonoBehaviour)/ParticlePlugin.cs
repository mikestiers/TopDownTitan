using UnityEngine;

namespace Playniax.ParticleSystem
{
    public class ParticlePlugin : MonoBehaviour
    {
        public static ParticlePlugin instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<ParticlePlugin>();

                return _instance;
            }
        }

        public virtual void OnPostProcess(Particle particle)
        {
        }

        static ParticlePlugin _instance;
    }
}