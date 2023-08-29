namespace Playniax.Ignition
{
    public interface IIgnitionBehaviour
    {
        bool ignitionInitialized
        {
            get;
            set;
        }

        void IgnitionInit();
    }
}