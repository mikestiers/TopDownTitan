using UnityEngine;
using UnityEngine.UI;
using Playniax.Ignition;

public class ObjectCounter_Example : MonoBehaviour
{
    public Text text;

    void Awake()
    {
        if (text == null) text = GetComponent<Text>();
    }

    void Update()
    {
        // Get and display the number of objects.
        text.text = ObjectCounter.count.ToString();
    }
}
