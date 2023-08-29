using UnityEngine;
using Playniax.Ignition;

namespace Playniax.Pyro
{
    public class PickupEnergyBeam : CollisionBase2D
    {
        [System.Serializable]
        public class MessengerSettings
        {
            public string activatedId = "Activated";
            public string reloadedId = "Reloaded";
        }
        public enum Mode { increase, straightToMax };

        public MessengerSettings messengerSettings;
        public GameObject prefab;
        public Mode mode;
        public int increase = 25;
        public int max = 100;
        public string activatedText = "Energy Beam Activated At %";
        public string reloadedText = "Energy Beam At %";

        public EnergyBeam energyBeam
        {
            get
            {
                return _energyBeam;
            }
        }
        public override void Awake()
        {
            base.Awake();

            if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);
        }

        public void Buy()
        {
            if (prefab == null) return;
            if (prefab.GetComponent<EnergyBeam>() == null) return;

            if (_energyBeam && _energyBeam.gameObject == null || _energyBeam == null) _energyBeam = Instantiate(prefab, transform.position, transform.rotation, transform).GetComponent<EnergyBeam>();

            _energyBeam.ttl = max;

            _energyBeam.gameObject.SetActive(true);
        }

        public override void OnCollision(CollisionBase2D collision)
        {
            if (collision.id != id) return;

            if (prefab && _energyBeam == null)
            {
                _energyBeam = Instantiate(prefab, transform.position, transform.rotation, transform).GetComponent<EnergyBeam>();

                if (_energyBeam == null) return;

                _energyBeam.gameObject.SetActive(true);

                if (mode == Mode.increase)
                {
                    _energyBeam.ttl += increase;

                    if (max > 0 && _energyBeam.ttl > max) _energyBeam.ttl = max;

                    if (Messenger.instance) Messenger.instance.Create(messengerSettings.activatedId, activatedText.Replace("%", MathHelpers.Dif(max, _energyBeam.ttl)), collision.transform.position);
                }
                else if (mode == Mode.straightToMax)
                {
                    _energyBeam.ttl = max;

                    if (Messenger.instance) Messenger.instance.Create(messengerSettings.activatedId, activatedText.Replace("%", MathHelpers.Dif(max, _energyBeam.ttl)), collision.transform.position);
                }
            }
            else if (prefab && _energyBeam != null)
            {
                if (mode == Mode.increase)
                {
                    _energyBeam.ttl += increase;

                    if (_energyBeam.ttl > max) _energyBeam.ttl = max;

                    if (Messenger.instance) Messenger.instance.Create(messengerSettings.reloadedId, reloadedText.Replace("%", MathHelpers.Dif(max, _energyBeam.ttl)), collision.transform.position);
                }
                else if (mode == Mode.straightToMax && max > 0)
                {
                    _energyBeam.ttl = max;

                    if (Messenger.instance) Messenger.instance.Create(messengerSettings.reloadedId, reloadedText.Replace("%", MathHelpers.Dif(max, _energyBeam.ttl)), collision.transform.position);
                }
            }

            Destroy(collision.gameObject);
        }

        EnergyBeam _energyBeam;
    }
}