using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Game.UI
{
    public class CallbackButton : MonoBehaviour
    {
        //Object Data
        private Action callback = null;

        [Header("Button")]
        [SerializeField] private Button button = null;

        //Properties
        public bool IsInteractable => button.interactable;

        private void Awake()
        {
            if (button == null)
                button = GetComponent<Button>();

            button.onClick.AddListener(OnButtonClick);
        }

        void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

        public void Initialize(Action callback = null)
        {
            this.callback = callback;
        }

        public virtual void OnButtonClick()
        {
            callback?.Invoke();
        }

        public void EnableButtonInteraction(bool enable) => button.interactable = enable;

#if UNITY_EDITOR
        protected void Reset()
        {
            if (TryGetComponent(out Button foundButton))
            {
                button = foundButton;

                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
#endif
    }
}