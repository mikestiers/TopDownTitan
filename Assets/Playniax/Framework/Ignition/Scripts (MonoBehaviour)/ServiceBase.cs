using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition
{
    // A ServiceBase based class resides dynamically in memory can can be called by any script.
    public class ServiceBase : MonoBehaviour
    {
        public static List<ServiceBase> services = new List<ServiceBase>();
        // Task id.
        public string id = "Explosion Red";
        public static void Call(string id)
        {
            for (int i = 0; i < services.Count; i++)
            {
                if (services[i].id == id)
                {
                    services[i].Call();
                }
            }
        }
        public static ServiceBase Get(string id)
        {
            for (int i = 0; i < services.Count; i++)
            {
                if (services[i].id == id) return services[i];
            }

            return null;
        }
        public virtual void Call()
        {
        }
        public virtual void OnEnable()
        {
            services.Add(this);
        }
        public virtual void OnDisable()
        {
            services.Remove(this);
        }
    }
}

