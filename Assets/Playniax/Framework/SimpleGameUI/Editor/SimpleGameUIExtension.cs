﻿using UnityEditor;
using UnityEngine;

namespace Playniax.UI.SimpleGameUI
{
    [InitializeOnLoad]
    public class SimpleGameUIExtension
    {
        static SimpleGameUIExtension()
        {
            Selection.selectionChanged += OnSelectionChange;
        }

        static void OnSelectionChange()
        {
            if (_simpleGameUI == null) _simpleGameUI = Object.FindObjectOfType<SimpleGameUI>();

            if (_simpleGameUI && Selection.activeGameObject)
            {
                for (int i = 0; i < _simpleGameUI.transform.childCount; i++)
                {
                    _simpleGameUI.transform.GetChild(i).gameObject.SetActive(false);
                }

                var parent = Selection.activeGameObject.transform;

                while (parent != null)
                {
                    for (int i = 0; i < _simpleGameUI.transform.childCount; i++)
                    {
                        if (_simpleGameUI.transform.GetChild(i).name == parent.name) parent.gameObject.SetActive(true);
                    }

                    parent = parent.parent;
                }
            }

        }

        static SimpleGameUI _simpleGameUI;
    }
}