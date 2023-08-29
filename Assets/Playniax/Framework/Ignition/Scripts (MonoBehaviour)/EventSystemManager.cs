using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Playniax.Ignition
{
    // Will disable any redundant EventSystem.
    public class EventSystemManager : MonoBehaviour
    {
        void Awake()
        {
            var list = FindObjectsOfType<EventSystem>();

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].enabled && i > 0) list[i].enabled = false;
            }
        }
    }
}