using System;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Game.UI
{
    public class LocaleButton : MonoBehaviour
    {
        //Object data
        private const string languagesStringTableKey = "Languages String Table";
        private string localeName = string.Empty;
        private Action callback = null;

        [Header("Self Contained References")]
        [SerializeField] private CallbackButton button = null;
        [SerializeField] private TextSetter buttonTextSetter = null;

        //Properties
        public TextSetter ButtonTextSetter => buttonTextSetter;

        private void Awake()
        {
            button.Initialize(OnButtonClick);
        }

        private void OnDestroy()
        {
            Delegate[] delegateList = callback?.GetInvocationList();
            if (delegateList == null) return;

            foreach (Delegate dlgt in delegateList)
                callback -= (Action)dlgt;
        }

        public void Initialize(string localeName, Action buttonCallback = null)
        {
            this.localeName = localeName;
            UpdateButtonText();

            if (buttonCallback == null) return;

            callback += buttonCallback;
        }

        private void OnButtonClick()
        {
            callback?.Invoke();
        }

        public void UpdateButtonText()
        {
            string buttonText = GetStringFromKey(languagesStringTableKey, localeName);
            buttonTextSetter.SetText(buttonText);
        }

        private static string GetStringFromKey(string localizationTableKey, string stringKey)
        {
            var currentLocale = LocalizationSettings.SelectedLocaleAsync.WaitForCompletion();
            var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(localizationTableKey, stringKey, currentLocale);
            return operation.WaitForCompletion();
        }
    }
}