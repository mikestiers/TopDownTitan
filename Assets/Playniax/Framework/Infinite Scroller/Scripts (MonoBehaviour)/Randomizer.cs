//using Playniax.Ignition;
//using Playniax.Pyro;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Playniax.InfiniteScroller
//{
//    [AddComponentMenu("Playniax/Prototyping/Infinite Scroller/Randomizer")]
//    public class Randomizer : IgnitionBehaviour
//    {
//        [Tooltip("Objects attached or created by the randmoizer.")]
//        public List<GameObject> objects = new List<GameObject>();
//        [Tooltip("List of prefabs to choose from.")]
//        public GameObject[] prefabs;
//        public int count;
//        [Tooltip("Renderer's order.")]
//        public int orderInLayer;
//        public float scale = 1;
//        public float z;
//        [Tooltip("Determines if an object is rotated.")]
//        public bool randomRotation;
//        [Tooltip("Determines the layer to use for the objects.")]
//        public ObjectsLayer layer;
//        public Vector3 safeZone;
//        public float safeZoneRadius = 3;
//        public float distance = 1;
//        public int failSafe = 1000;

//#if UNITY_EDITOR
//        void Reset()
//        {
//            layer = GetComponent<ObjectsLayer>();

//            if (layer == null && transform.parent) layer = transform.parent.GetComponentInParent<ObjectsLayer>();
//        }
//#endif
//        public override void IgnitionInit()
//        {
//            for (int i = 0; i < prefabs.Length; i++)
//                if (prefabs[i] && prefabs[i].scene.rootCount > 0) prefabs[i].SetActive(false);
//        }
//        public override void Awake()
//        {
//            base.Awake();

//            if (layer == null) layer = GetComponent<ObjectsLayer>();

//            if (layer == null && transform.parent) layer = transform.parent.GetComponentInParent<ObjectsLayer>();
//        }
//        void Start()
//        {
//            if (count == 0) count = prefabs.Length;

//            for (int i = 0; i < count; i++)
//            {
//                var prefab = _GetPrefab();

//                var obj = Instantiate(prefab);

//                RendererHelpers.SetOrder(obj, orderInLayer);

//                var layerRenderer = obj.AddComponent<LayerRenderer>();

//                layerRenderer.layer = layer;

//                obj.transform.localScale *= scale;

//                _Position(layerRenderer);

//                obj.transform.SetParent(transform);

//                if (randomRotation) obj.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 359));
//            }
//        }

//        GameObject _GetPrefab()
//        {
//            var prefab = prefabs[_index];

//            _index += 1;

//            if (_index >= prefabs.Length) _index = 0;

//            return prefab;
//        }

//        Vector3 _GetRange(Vector2 size)
//        {
//            var range = layer.scroller.size * .5f;

//            var layers = layer.scroller.GetLayers();

//            var speed = layers[0].speed;

//            for (int i = 0; i < layers.Length; i++)
//                if (layers[i].speed < speed) speed = layers[i].speed;

//            range /= speed;

//            range *= layer.speed;

//            range -= (Vector3)size * .5f;

//            var position = Vector3.zero;

//            position.x = Random.Range(-range.x, range.x);
//            position.y = Random.Range(-range.y, range.y);

//            position.z = z;

//            return position;
//        }

//        void _Position(LayerRenderer layerRenderer)
//        {
//            layerRenderer.transform.position = _GetRange(layerRenderer.bounds.size);

//            while (BoundsHelper.IsFreeSpace(layerRenderer, safeZone, safeZoneRadius, distance) == false && _fails < failSafe)
//            {
//                layerRenderer.transform.position = _GetRange(layerRenderer.bounds.size);

//                _fails += 1;
//            }

//            if (_fails < failSafe)
//            {
//                layerRenderer.gameObject.SetActive(true);

//                objects.Add(layerRenderer.gameObject);
//            }
//            else
//            {
//                Destroy(layerRenderer.gameObject);

//                return;
//            }
//        }

//        int _fails;
//        int _index;
//    }
//}
