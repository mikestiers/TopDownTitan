using Playniax.Ignition;
using Playniax.Pyro;
using Playniax.UI.SimpleGameUI;

namespace Playniax.Sequencer
{
    public class SequenceBase : SpawnerBase
    {
        public int state { get; set; }
        public SequencerBase sequencer { get; set; }
        public override void Awake()
        {
            base.Awake();

            if (_GetSequencer() == null)
            {
                OnSequencerAwake();

                state = 1;
            }
        }

        public virtual void OnSequencerInit()
        {
        }

        public virtual void OnSequencerAwake()
        {
        }

        public virtual void OnSequencerUpdate()
        {
        }

        protected void Update()
        {
            if (Timing.Paused) return;
            if (SimpleGameUI.instance && SimpleGameUI.instance.isBusy) return;
            if (SimpleGameUI.instance && SimpleGameUI.instance.screenSettings.effects.messenger.isActiveAndEnabled) return;
            if (TextEffect.current) return;

            if (state > 0) OnSequencerUpdate();
        }

        SequencerBase _GetSequencer()
        {
            var parent = transform.parent;

            while (parent)
            {
                var sequencerBase = parent.GetComponent<SequencerBase>();

                if (sequencerBase) return sequencerBase;

                parent = parent.parent;
            }

            return null;
        }
    }
}
