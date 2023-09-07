//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;
//using Playniax.Ignition;
//using Playniax.Pyro;

//namespace Playniax.Randomizers
//{
//    public class RandomLevelGenerator : EngineBehaviour
//    {
//        public GameObject[] prefab;
//        public int count = 10;
//        public Vector3 safeZone;
//        public float safeZoneRadius = 3;
//        public int failSafe = 1000;
//        public float distance = 1;
//        public Tilemap tilemap;
//        public bool useTilemapBounds;
//        public Vector3 bounds = new Vector3(5, 5, 0);
//#if UNITY_EDITOR
//        [Tooltip("Gizmos color settings.")]
//        public Color gizmoColor = new Color(0, 1, 0, 0.5f);
//        [Tooltip("Whether to show the gizmos or not.")]
//        public bool showGizmos = true;
//#endif

//#if UNITY_EDITOR
//        void OnDrawGizmos()
//        {
//            if (showGizmos == false) return;

//            Gizmos.color = gizmoColor;

//            Gizmos.DrawWireSphere(safeZone, safeZoneRadius);
//        }
//#endif
//        public override void OnAwake()
//        {
//            for (int i = 0; i < prefab.Length; i++)
//                if (prefab[i] && prefab[i].scene.rootCount > 0) prefab[i].SetActive(false);
//        }

//        public override void OnStart()
//        {
//            Generate();
//        }
//        public void Generate()
//        {
//            BoundsHelper helper = null;
//            List<Bounds> tileBounds = null;

//            if (tilemap) tileBounds = TilemapHelpers.GetBounds(tilemap);

//            for (int i = 0; i < count; i++)
//            {
//                var pick = Random.Range(0, prefab.Length);

//                var x = Random.Range(-_GetBounds().x, _GetBounds().x);
//                var y = Random.Range(-_GetBounds().y, _GetBounds().y);
//                var z = Random.Range(-_GetBounds().z, _GetBounds().z);

//                if (tilemap)
//                {
//                    var clone = Instantiate(prefab[pick], tilemap.transform);

//                    helper = clone.GetComponent<BoundsHelper>();

//                    if (helper == null) helper = clone.AddComponent<BoundsHelper>();

//                    helper.transform.localPosition = new Vector3(x, y, z);

//                    helper.gameObject.SetActive(true);

//                    var animator = clone.GetComponent<Animator>();
//                    if (animator) animator.Play(0, -1, Random.Range(0, 1f));
//                }
//                else
//                {
//                    var clone = Instantiate(prefab[pick], transform);

//                    helper = clone.GetComponent<BoundsHelper>();

//                    if (helper == null) helper = clone.AddComponent<BoundsHelper>();

//                    helper.transform.localPosition = new Vector3(x, y, z);

//                    helper.gameObject.SetActive(true);

//                    var animator = clone.GetComponent<Animator>();
//                    if (animator) animator.Play(0, -1, Random.Range(0, 1f));
//                }

//                while (BoundsHelper.IsFreeSpace(helper, safeZone, safeZoneRadius, distance) == false || (tilemap && FreeTileSpace() == false && failSafe > 0))
//                {
//                    x = Random.Range(-_GetBounds().x, _GetBounds().x);
//                    y = Random.Range(-_GetBounds().y, _GetBounds().y);
//                    z = Random.Range(-_GetBounds().z, _GetBounds().z);

//                    helper.transform.position = new Vector3(x, y, z);

//                    failSafe -= 1;
//                }

//                if (failSafe <= 0)
//                {
//                    Destroy(helper.gameObject);

//                    return;
//                }

//                /*
//                bool FreeSpace()
//                {
//                    var spawned = FindObjectsOfType<BoundsHelper>();

//                    for (int j = 0; j < spawned.Length; j++)
//                    {
//                        if (spawned[j].gameObject == helper.gameObject) continue;
//                        if (Vector3.Distance(safeZone, helper.transform.position) < safeZoneRadius) return false;
//                        if (Vector3.Distance(helper.transform.position, spawned[j].transform.position) < distance) return false;

//                        var bounds1 = helper.GetBounds();
//                        var bounds2 = spawned[j].GetBounds();

//                        //if (bounds1.size.magnitude > 0 && bounds2.size.magnitude > 0 && bounds1.Intersects(bounds2)) return false;
//                        if (bounds1.Intersects(bounds2)) return false;
//                    }

//                    return true;
//                }
//                */

//                bool FreeTileSpace()
//                {
//                    for (int j = 0; j < tileBounds.Count; j++)
//                    {
//                        var bounds1 = helper.bounds;
//                        var bounds2 = tileBounds[j];

//                        if (Vector3.Distance(bounds1.center, bounds2.center) < distance) return false;

//                        if (bounds1.Intersects(bounds2)) return false;
//                    }

//                    return true;
//                }
//            }
//        }

//        Vector3 _GetBounds()
//        {
//            if (tilemap && useTilemapBounds)
//            {
//                var width = Mathf.Max(tilemap.cellBounds.xMin, tilemap.cellBounds.xMax);
//                var height = Mathf.Max(tilemap.cellBounds.yMin, tilemap.cellBounds.yMax);

//                return new Vector3(width * tilemap.cellSize.x, height * tilemap.cellSize.y);
//            }

//            return bounds;
//        }
//    }
//}