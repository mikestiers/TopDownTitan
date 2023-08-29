using Playniax.ParticleSystem;

namespace Playniax.Pyro
{
    public class PickupNuke : CollisionBase2D
    {
        public string emitter = "Nuke";

        public override void OnCollision(CollisionBase2D collision)
        {
            if (collision.id != id) return;

            Nukeable.Nuke();

            EmitterGroup.Call(emitter, default);

            Destroy(collision.gameObject);
        }
    }
}