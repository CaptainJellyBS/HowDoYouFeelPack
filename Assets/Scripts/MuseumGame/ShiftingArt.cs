using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HowDoYouFeel.Global;
using TMPro;

namespace HowDoYouFeel.MuseumGame
{
    public enum ArtStyle { Baroque, Impressionist, Renaissance, Romantic, Mixed}

    public class ShiftingArt : MonoBehaviour
    {
        public ArtStyle style;

        [Header("Internal Gameobjects")]
        public TextMeshProUGUI infoTMP;
        public SpriteRenderer currentArtSprite, newArtSprite;
        public Transform currentInfoSign, newInfoSign;

        ArtContainer currentArtContainer, newArtContainer;
        bool isCurrentArtVisible, isCurrentSignVisible, isNewArtVisible, isNewSignVisible;
        bool canChange = false;
        ArtManager artManager;

        private void Start()
        {
            Initialize(style);
        }

        public void Initialize(ArtStyle _style)
        {
            if(artManager == null) { artManager = GameManager.Instance.GetComponent<ArtManager>(); }
            style = _style;
            GenerateCurrentArt();
            GenerateNewArt();
            canChange = currentArtSprite.GetComponent<Renderer>().isVisible;
        }

        #region Shifting mechanics
        public void CheckChange()
        {
            if (!canChange) { return; }
            if(isCurrentArtVisible || isCurrentSignVisible || isNewArtVisible || isNewSignVisible) { return; }

            //We can change and the art is invisible, so change
            canChange = false; //We don't want the art to be constantly shifting while invisible

            currentArtContainer = newArtContainer;
            currentArtSprite.transform.localScale = newArtSprite.transform.localScale;
            currentInfoSign.transform.position = newInfoSign.transform.position;

            UpdateCurrentArt();
            GenerateNewArt();
        }

        public void UpdateCurrentArt()
        {
            currentArtSprite.sprite = currentArtContainer.sprite;
            infoTMP.text = currentArtContainer.GetDescription();
            float infoXPos = currentArtSprite.transform.localPosition.x -
                (0.5f * currentArtSprite.transform.localScale.x * (currentArtSprite.sprite.rect.size.x / currentArtSprite.sprite.pixelsPerUnit)
                + 0.16f + 0.1f);
            currentInfoSign.localPosition = new Vector3(infoXPos, currentInfoSign.localPosition.y, currentInfoSign.localPosition.z);
        }

        public void UpdateNewArt()
        {
            newArtSprite.sprite = newArtContainer.sprite;
            float infoXPos = newArtSprite.transform.localPosition.x -
                (0.5f * newArtSprite.transform.localScale.x * (newArtSprite.sprite.rect.size.x / newArtSprite.sprite.pixelsPerUnit)
                + 0.16f + 0.1f);
            newInfoSign.localPosition = new Vector3(infoXPos, newInfoSign.localPosition.y, newInfoSign.localPosition.z);
        }

        public void GenerateCurrentArt()
        {
            currentArtContainer = artManager.GetNext(style);
            currentArtSprite.transform.localScale = Vector3.one * Random.Range(currentArtContainer.MinScale, currentArtContainer.MaxScale);
            UpdateCurrentArt();
        }

        public void GenerateNewArt()
        {
            newArtContainer = artManager.GetNext(style);
            newArtSprite.transform.localScale = Vector3.one * Random.Range(newArtContainer.MinScale, newArtContainer.MaxScale);
            UpdateNewArt();
        }


        #endregion
        #region event messes
        public void SetCurrentArtVisibility(bool isVisible)
        {
            isCurrentArtVisible = isVisible;
            if (isVisible) { canChange = true; }

            CheckChange();
        }

        public void SetCurrentSignVisibility(bool isVisible)
        {
            isCurrentSignVisible = isVisible;

            CheckChange();
        }

        public void SetNewArtVisibility(bool isVisible)
        {
            isNewArtVisible = isVisible;

            CheckChange();
        }

        public void SetNewSignVisibility(bool isVisible)
        {
            isNewSignVisible = isVisible;

            CheckChange();
        }
        #endregion
    }
}
