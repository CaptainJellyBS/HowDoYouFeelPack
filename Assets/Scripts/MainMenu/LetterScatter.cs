using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using HowDoYouFeel.Global;

namespace HowDoYouFeel.MainMenu
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LetterScatter : MonoBehaviour
    {
        TextMeshProUGUI origText;
        public GameObject textPrefab;

        RectTransform[] letterTransforms;
        Vector2[] positions;
        int[] tp;

        [Range(0.0f, 1.0f)]
        public float scatterIntensity;
        public Color letterColor;

        private void Start()
        {
            origText = GetComponent<TextMeshProUGUI>();

            letterTransforms = new RectTransform[origText.text.Length];
            positions = new Vector2[origText.text.Length];
            tp = new int[origText.text.Length];
            float offset = 0.0f;
            TMP_FontAsset font = origText.font;
            float charSize = origText.fontSize;

            font.sourceFontFile.RequestCharactersInTexture(origText.text, (int)charSize, FontStyle.Normal);

            for (int i = 0; i < origText.text.Length; i++)
            {
                tp[i] = i;

                GameObject textO = Instantiate(textPrefab, transform.parent);
                letterTransforms[i] = textO.GetComponent<RectTransform>();

                TextMeshProUGUI oTMP = textO.GetComponent<TextMeshProUGUI>();
                oTMP.font = font;
                oTMP.fontSize = charSize;
                oTMP.text = origText.text[i].ToString();
                oTMP.color = origText.color;

                //oTMP.transform.localPosition = origText.transform.localPosition + (offset * Vector3.right);
                positions[i] = origText.rectTransform.anchoredPosition + (Vector2.right * offset);

                CharacterInfo charInf;
                Debug.Log(font.sourceFontFile.GetCharacterInfo(origText.text[i], out charInf, (int)charSize, FontStyle.Normal));
                Debug.Log(charInf.advance);
                offset += charInf.advance;
            }

            Utility.FisherYates(ref tp);
            origText.enabled = false;
        }


        private void Update()
        {
            for (int i = 0; i < origText.text.Length; i++)
            {
                RectTransform r = letterTransforms[i];
                r.anchoredPosition = Vector2.Lerp(positions[i], positions[tp[i]], scatterIntensity);
                r.GetComponent<TextMeshProUGUI>().color = letterColor;
            }
        }
    }
}
