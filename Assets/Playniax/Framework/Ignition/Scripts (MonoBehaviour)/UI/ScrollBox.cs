using UnityEngine;
using UnityEngine.UI;
using Playniax.Ignition;

namespace Playniax.Ignition.UI
{
    // The scrollbox supports text, url links and images and it has its own scripting language. 
    public class ScrollBox : MonoBehaviour
    {
        public TextAsset externalScript;
        public bool useExternalScript;

        [TextArea(15, 20)]
        public string script;
        public string start;
        public string dataBreak = "&";
        public string lineBreak = "|";
        public bool allCaps = false;
        public AssetBank assetBank;
        public GameObject content;
        public float contentHeight;

        // Whether the mouse is hovering the scrollbox or not.
        public bool isMouseOver
        {
            get
            {
                var mousePosition = _rectTransform.InverseTransformPoint(Input.mousePosition);
                if (_rectTransform.rect.Contains(mousePosition)) return true;
                return false;
            }
        }

        // Returns GameObject if it contains value.
        public GameObject Contains(string value)
        {
            foreach (Transform child in content.transform)
            {
                if (child.name.Contains(value)) return child.gameObject;
            }

            return null;
        }

        // Sets the scrollbox position by value.
        public void SetPosition(string value)
        {
            var child = Contains(value);
            if (child == null) return;

            var position = new Vector3(child.transform.localPosition.x, -child.transform.localPosition.y, child.transform.localPosition.z);

            position.y += _rectTransform.sizeDelta.y * .5f;
            position.y -= child.GetComponent<Text>().fontSize * .5f;

            content.transform.localPosition = position;
        }

        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            content = new GameObject("Content");

            content.transform.SetParent(transform, false);
        }

        void Start()
        {
            if (useExternalScript && externalScript)
            {
                contentHeight = PageEngine.ExecuteScript(content.transform, _rectTransform.sizeDelta, externalScript.text, assetBank, allCaps, dataBreak, lineBreak);
            }
            else
            {
                contentHeight = PageEngine.ExecuteScript(content.transform, _rectTransform.sizeDelta, script, assetBank, allCaps, dataBreak, lineBreak);
            }

            if (start != "") SetPosition(start);
        }

        RectTransform _rectTransform;
    }
}