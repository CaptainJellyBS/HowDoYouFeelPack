using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HowDoYouFeel.MuseumGame
{
    [CreateAssetMenu(fileName = "ArtContainer", menuName = "ScriptableObjects/MuseumGameArtContainer")]
    public class ArtContainer : ScriptableObject
    {
        public Sprite sprite;
        public string artName, creationYear, artistName, artMedium, artistBirthYear, artistDeathYear;
        public ArtStyle style;

        public float MaxScale
        {
            get 
            {
                return Mathf.Min(
                    (GameManager.Instance ? GameManager.Instance.maxArtXSize : 3.0f) / (sprite.rect.size.x / sprite.pixelsPerUnit),
                    (GameManager.Instance ? GameManager.Instance.maxArtYSize : 3.5f) / (sprite.rect.size.y / sprite.pixelsPerUnit));
            }                           
        }

        public float MinScale
        {
            get
            {
                return Mathf.Max(
                    (GameManager.Instance ? GameManager.Instance.minArtXSize : 0.25f) / (sprite.rect.size.x / sprite.pixelsPerUnit),
                    (GameManager.Instance ? GameManager.Instance.minArtYSize : 0.25f) / (sprite.rect.size.y / sprite.pixelsPerUnit));
            }
        }
        public string GetDescription()
        {
            return "<b>" + artName + "</b>, " + creationYear + "\n"
                + artistName + " (" + artistBirthYear + " - " + artistDeathYear + ")\n" +
                "<i>" + artMedium + "</i>";
        }
    }
}
