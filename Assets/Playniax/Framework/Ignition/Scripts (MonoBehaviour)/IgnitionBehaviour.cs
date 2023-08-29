using System.Linq;
using UnityEngine;

namespace Playniax.Ignition
{
    public class IgnitionBehaviour : MonoBehaviour, IIgnitionBehaviour
    {
        public static void Initialize(IIgnitionBehaviour instance)
        {
            if (_instance)
            {
                if (instance.ignitionInitialized == false)
                {
                    instance.IgnitionInit();

                    instance.ignitionInitialized = true;
                }
            }
            else
            {
                var list = FindObjectsOfType<MonoBehaviour>().OfType<IIgnitionBehaviour>();

                foreach (IIgnitionBehaviour obj in list)
                {
                    if (obj.ignitionInitialized == false)
                    {
                        obj.IgnitionInit();

                        obj.ignitionInitialized = true;
                    }
                }

                _instance = new GameObject("IgnitionBehaviour = " + list.Count());

                //_instance.hideFlags = HideFlags.HideInHierarchy;
            }
        }

        public bool ignitionInitialized
        {
            get;
            set;
        }
        public virtual void Awake()
        {
            Initialize(this);
        }

        public virtual void IgnitionInit()
        {
        }

        static GameObject _instance;
    }
}