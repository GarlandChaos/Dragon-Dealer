using UnityEditor;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

namespace Game.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextSetter : MonoBehaviour
    {
        [Header("Object Data")]
        [SerializeField, TextArea] protected string noTextString = string.Empty;

        [Header("Character limit: if greater than 0, limits characters")]
        [SerializeField, Min(0)] protected int characterLimit = 0;

        [Header("Self Contained References")]
        [SerializeField] private TMP_Text text = null;

        public virtual void OnValidate()
        {
            if (text == null) text = GetComponent<TMP_Text>();
        }

        public virtual void Awake()
        {
            if (text == null) text = GetComponent<TMP_Text>();
        }

        //Properties
        public string Text => text.text;

        public virtual void SetText(string textString)
        {
            if (characterLimit > 0)
                textString = textString.Substring(0, textString.Length > characterLimit ? characterLimit : textString.Length);

            text.SetText(textString);
        }

        public void ForceMeshUpdate()
            => text.ForceMeshUpdate();

        [Button("Reset Text")]
        public virtual void ResetText()
        {
            text.SetText(noTextString);
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            if (TryGetComponent(out TMP_Text foundText))
            {
                text = foundText;

                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
#endif
    }
}