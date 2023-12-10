using HowDoYouFeel.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowDoYouFeel.MuseumGame
{
    public class ArtManager : MonoBehaviour
    {
        public ArtContainer[] baroqueArt, impressionistArt, renaissanceArt, romanticArt;
        int baroqueIndex = 0, impressionistIndex = 0, renaissanceIndex = 0, romanticIndex = 0;

        public ArtContainer GetNext(ArtStyle style)
        {
            ArtContainer result;
            switch(style)
            {
                case ArtStyle.Mixed: return GetNext((ArtStyle)Random.Range(0, 4));

                case ArtStyle.Baroque:
                    result = baroqueArt[baroqueIndex];
                    baroqueIndex++; 
                    if(baroqueIndex >= baroqueArt.Length) { baroqueIndex = 0; Utility.FisherYates(ref baroqueArt); }
                    return result;

                case ArtStyle.Impressionist: 
                    result = impressionistArt[impressionistIndex];
                    impressionistIndex++; 
                    if(impressionistIndex >= impressionistArt.Length) { impressionistIndex = 0; Utility.FisherYates(ref impressionistArt); }
                    return result;

                case ArtStyle.Renaissance:
                    result = renaissanceArt[renaissanceIndex];
                    renaissanceIndex++;
                    if(renaissanceIndex >= renaissanceArt.Length) { renaissanceIndex = 0; Utility.FisherYates(ref renaissanceArt); }
                    return result;

                case ArtStyle.Romantic:
                    result = romanticArt[romanticIndex];
                    romanticIndex++;
                    if(romanticIndex >= romanticArt.Length) { romanticIndex = 0;Utility.FisherYates(ref romanticArt); }
                    return result;

                default: throw new System.ArgumentException("Ayo bro don't go inventing new artstyles yo");
            }
        }
    }
}
