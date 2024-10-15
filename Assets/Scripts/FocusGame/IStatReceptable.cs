using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.FocusGame
{
    public interface IStatReceptable
    {
        public void ReceiveParticle(StatParticle particle);
    }
}
