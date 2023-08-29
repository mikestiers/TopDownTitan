using UnityEngine;

namespace Playniax.Pyro
{
    public class PickupDrone : CollisionBase2D
    {
        [System.Serializable]
        public class MessengerSettings
        {
            public string activatedId = "Activated";
        }
        public MessengerSettings messengerSettings;
        public GameObject prefab;
        public string activatedText = "Drone Activated";
        public override void Awake()
        {
            base.Awake();

            if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);
        }

        public void Buy()
        {
            if (prefab == null) return;
            if (prefab.GetComponent<Drone>() == null) return;

            var drone = Instantiate(prefab, transform.position, transform.rotation).GetComponent<Drone>();

            drone.controller = gameObject;

            drone.gameObject.SetActive(true);
        }
        public override void OnCollision(CollisionBase2D collision)
        {
            if (prefab == null) return;
            if (prefab.GetComponent<Drone>() == null) return;

            if (collision.id != id) return;

            var drone = Instantiate(prefab, transform.position, transform.rotation).GetComponent<Drone>();

            drone.controller = gameObject;

            drone.gameObject.SetActive(true);

            if (Messenger.instance) Messenger.instance.Create(messengerSettings.activatedId, activatedText, collision.transform.position);

            Destroy(collision.gameObject);
        }
    }
}