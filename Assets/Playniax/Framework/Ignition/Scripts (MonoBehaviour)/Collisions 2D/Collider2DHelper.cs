using System.Collections.Generic;
using UnityEngine;

public class Collider2DHelper : MonoBehaviour
{
    public string group;
    public Collider2D[] colliders = new Collider2D[0];

    public static Collider2D GetAny(Collider2D collider)
    {
        if (collider == null) return null;
        if (collider.gameObject == null) return null;
        if (collider.gameObject.activeInHierarchy == false) return null;

        for (int i = 0; i < _helpers.Count; i++)
            for (int j = 0; j < _helpers[i].colliders.Length; j++)
                if (collider != _helpers[i].colliders[j] && collider.Distance(_helpers[i].colliders[j]).isOverlapped) return _helpers[i].colliders[j];

        return null;
    }

    public static Collider2D GetAny(Collider2D collider, string group)
    {
        if (collider == null) return null;
        if (collider.gameObject == null) return null;
        if (collider.gameObject.activeInHierarchy == false) return null;

        for (int i = 0; i < _helpers.Count; i++)
            for (int j = 0; j < _helpers[i].colliders.Length; j++)
                if (collider != _helpers[i].colliders[j] && group == _helpers[i].group && collider.Distance(_helpers[i].colliders[j]).isOverlapped) return _helpers[i].colliders[j];

        return null;
    }

    public Collider2D GetAny()
    {
        for (int i = 0; i < _helpers.Count; i++)
            for (int j = 0; j < _helpers[i].colliders.Length; j++)
                for (int k = 0; k < colliders.Length; k++)
                    if (colliders[k] != _helpers[i].colliders[j] && group == _helpers[i].group && colliders[k].Distance(_helpers[i].colliders[j]).isOverlapped) return _helpers[i].colliders[j];

        return null;
    }

    void OnEnable()
    {
        _helpers.Add(this);
    }

    void OnDisbale()
    {
        _helpers.Remove(this);
    }

    static List<Collider2DHelper> _helpers = new List<Collider2DHelper>();
}
