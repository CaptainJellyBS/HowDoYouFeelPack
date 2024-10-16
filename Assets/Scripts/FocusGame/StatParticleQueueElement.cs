using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowDoYouFeel.FocusGame
{
    public struct StatParticleQueueElement
    {
        public IStatReceptable target;
        public int statValue;
        public StatParticleType statType;

        public StatParticleQueueElement(IStatReceptable _target, int _statValue, StatParticleType _statType)
        {
            target = _target;
            statValue = _statValue;
            statType = _statType;
        }
    }
}
