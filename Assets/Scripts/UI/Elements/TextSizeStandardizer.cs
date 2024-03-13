using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class TextSizeStandardizer : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> textBoxesToStandardize = new();

        [SerializeField] private TMP_Text manualOverride = null;

        private void OnEnable()
        {
            StandardizeFontSizes();
        }

        private float GetMinFontSize()
        {
            if (manualOverride != null)
            {
                float manualSize = manualOverride.fontSize;
                return manualSize;
            }

            float fontSize = float.MaxValue;
            foreach (TMP_Text textbox in textBoxesToStandardize)
            {
                if (textbox.fontSize < fontSize)
                    fontSize = textbox.fontSize;
            }

            return fontSize;
        }

        public void StandardizeFontSizes()
        {
            float fontSize = GetMinFontSize();
            foreach (TMP_Text textbox in textBoxesToStandardize)
            {
                textbox.fontSizeMax = fontSize;
                textbox.ForceMeshUpdate();
            }
        }

        public void AddToList(TMP_Text text)
        {
            textBoxesToStandardize.Add(text);
        }
    }
}