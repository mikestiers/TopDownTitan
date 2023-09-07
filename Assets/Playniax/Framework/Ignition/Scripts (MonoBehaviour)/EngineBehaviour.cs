using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Playniax.Ignition
{
    public class EngineBehaviour : MonoBehaviour
    {
        public  int order;
        public virtual void OnAwake() { }
        public virtual void OnStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnFixedUpdate() { }
        protected void Awake()
        {
            _Update();

            if (_main == this)
            {
                for (int i = 0; i < _list.Count(); i++)
                    _list.ElementAt(i).OnAwake();
            }
        }

        protected void Start()
        {
            _Update();

            if (_main == this)
            {
                for (int i = 0; i < _list.Count(); i++)
                    if (_list.ElementAt(i).enabled) _list.ElementAt(i).OnStart();
            }
        }

        protected void Update()
        {
            _Update();

            if (_main == this)
            {
                for (int i = 0; i < _list.Count(); i++)
                    if (_list.ElementAt(i).enabled) _list.ElementAt(i).OnUpdate();
            }
        }
        protected void LateUpdate()
        {
            _Update();

            if (_main == this)
            {
                for (int i = 0; i < _list.Count(); i++)
                    if (_list.ElementAt(i).enabled) _list.ElementAt(i).OnLateUpdate();
            }
        }
        protected void FixedUpdate()
        {
            _Update();

            if (_main == this)
            {
                for (int i = 0; i < _list.Count(); i++)
                    if (_list.ElementAt(i).enabled) _list.ElementAt(i).OnFixedUpdate();
            }
        }

        void _Update()
        {
            if (_main == null && enabled) _main = this;

            if (_main == this && enabled)
            {
                _list = FindObjectsOfType<EngineBehaviour>().OfType<EngineBehaviour>();

                _list = _list.OrderBy(w => w.order);
            }
        }

        static IEnumerable<EngineBehaviour> _list;
        static EngineBehaviour _main;
    }
}