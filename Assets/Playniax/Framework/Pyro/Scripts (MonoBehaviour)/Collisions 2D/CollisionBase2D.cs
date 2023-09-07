// https://www.jacksondunstan.com/articles/3753

using UnityEngine;

namespace Playniax.Pyro
{
    public class CollisionBase2D : MonoBehaviour
    {
        public Collider2D[] colliders = new Collider2D[0];
        public string id;
        public string group = "Enemy";
        public int delay = 1;
        public int frameStart { get; set; }

        public virtual void Awake()
        {
            if (colliders.Length == 0) colliders = GetComponentsInChildren<Collider2D>();

            frameStart = Time.frameCount + delay;
        }
        public virtual bool isAllowed
        {
            get { return true; }
        }
        public virtual bool isSafe
        {
            get 
            {
                if (gameObject == null) return false;
                if (isActiveAndEnabled == false) return false;
                if (isSuspended == true) return false;
                if (isAllowed == false) return false;

                return true;
            }
        }
        public bool isSuspended
        {
            get { return _suspended; }
            set { _suspended = value; }
        }
        public virtual void OnEnable()
        {
            if (colliders.Length > 0) CollisionMonitor2D.Add(group, this);
        }
        public virtual void OnDisable()
        {
            CollisionMonitor2D.Remove(group, this);

            _suspended = false;
        }
        public virtual void OnCollision(CollisionBase2D collision)
        {
        }

        bool _suspended;
    }
}