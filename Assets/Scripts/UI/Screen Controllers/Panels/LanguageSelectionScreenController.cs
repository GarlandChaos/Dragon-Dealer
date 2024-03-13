using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using Game.Utility.Pool;

namespace Game.UI
{
    public class LanguageSelectionScreenController : APanelScreenController
    {
        //Object data
        private bool initialized = false;
        private List<LocaleButton> localeButtonList = new();
        
        [Header("Self Contained References")]
        [SerializeField] private LocaleButtonPool localeButtonPool = null;
        [SerializeField] private Transform localeButtonContainerTransform = null;
        [SerializeField] private TextSizeStandardizer textSizeStandardizer = null;

        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            if (initialized) return;

            AsyncOperationHandle operation = Addressables.InitializeAsync();
            await operation.Task;

            AsyncOperationHandle initOperation = LocalizationSettings.InitializationOperation;
            await initOperation.Task;
            
            AsyncOperationHandle preloadOperation = LocalizationSettings.AssetDatabase.PreloadOperation;
            await preloadOperation.Task;

            List<Locale> localeList = LocalizationSettings.AvailableLocales.Locales;
            foreach (Locale locale in localeList)
            {
                LocaleButton localeButton = localeButtonPool.Pool.Get();
                int localeIndex = localeList.IndexOf(locale);
                localeButton.Initialize(locale.LocaleName, () => ChangeLocale(localeIndex));
                localeButton.transform.SetParent(localeButtonContainerTransform, false);
                localeButtonList.Add(localeButton);
                textSizeStandardizer.AddToList(localeButton.ButtonTextSetter.TMP_Text);
            }

            textSizeStandardizer.StandardizeFontSizes();

            initialized = true;
        }

        private void ChangeLocale(int localeId)
        {
            Locale locale = LocalizationSettings.AvailableLocales.Locales[localeId];
            LocalizationSettings.SelectedLocale = locale;

            UpdateLocaleButtonTexts();
        }

        private void UpdateLocaleButtonTexts()
        {
            foreach (LocaleButton localeButton in localeButtonList)
                localeButton.UpdateButtonText();

            UIManager.Instance.RequestScreen(ScreenIds.MAIN_MENU_SCREEN, true);
            Hide();
        }
    }
}