using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class UIManager : ASingleton<UIManager>
    {
        //Object Data
        public static string firstScreen = ScreenIds.MAIN_MENU_SCREEN;

        [Header("Asset References")]
        [SerializeField] private UISettings settings = null; //Contains screen prefabs and other settings

        [Header("Self Contained References")]
        [SerializeField] private PanelLayer panelLayer = null; //Gameobject that will hold all the panel screen controllers
        [SerializeField] private DialogLayer dialogLayer = null; //Gameobject that will hold all the dialog screen controllers
        [SerializeField] private Camera cameraUI = null;
        [SerializeField] private Canvas canvasUI = null;
        [SerializeField] private GraphicRaycaster graphicRaycaster = null;

        //Properties
        public Camera CameraUI => cameraUI;
        public Canvas CanvasUI => canvasUI;
        public GraphicRaycaster GraphicRaycaster => graphicRaycaster;

        private void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Instantiates all screens from settings and assign each one to the respective layer.
        /// </summary>
        private void Initialize()
        {
            foreach (GameObject screen in settings.Screens)
            {
                GameObject screenInstance = Instantiate(screen);

                if (settings.HideScreensUponInstantiation)
                    screenInstance.SetActive(screen.name == firstScreen ? true : false);

                IScreenController screenController = screenInstance.GetComponent<IScreenController>();

                if (screenController != null)
                {
                    IPanelScreenController panelScreenController = screenController as IPanelScreenController;
                    if (panelScreenController != null)
                    {
                        panelLayer.RegisterScreen(screen.name, panelScreenController, screenInstance.transform);
                        continue;
                    }

                    IDialogScreenController dialogScreenController = screenController as IDialogScreenController;
                    if (dialogScreenController != null)
                    {
                        dialogLayer.RegisterScreen(screen.name, dialogScreenController, screenInstance.transform);
                        continue;
                    }
                }
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.buildIndex == 0)
                RequestScreen(firstScreen, true);
        }

        /// <summary>
        /// Requests a screen with the given screenID to show or hide.
        /// </summary>
        /// <param name="screenID">The screen ID to request a screen.</param>
        /// <param name="show">If true, show the screen, otherwise hide it.</param>
        /// <param name="onCompleteCallback">Action to be passed to screen and invoked after shown.</param>
        /// <param name="values">Optional parameters to send when showing a screen. Each screen must treat them accordingly.</param> 
        public void RequestScreen(string screenID, bool show = true, Action onCompleteCallback = null, params object[] values)
        {
            if (panelLayer.TryToGetScreenById(screenID, out IPanelScreenController panelScreenController))
            {
                if (show)
                    panelLayer.ShowScreen(screenID, onCompleteCallback, values);
                else
                    panelLayer.HideScreen(screenID);
            }
            else if (dialogLayer.HasScreen(screenID))
            {
                if (show)
                    dialogLayer.ShowScreen(screenID, onCompleteCallback, values);
                else
                    dialogLayer.HideScreen(screenID);
            }
#if UNITY_EDITOR
            else
                throw new System.Exception(screenID + " is not registered in any layer.");
#endif
        }

        /// <summary>
        /// Hide all screens from all layers.
        /// </summary>
        public void HideAllScreens()
        {
            panelLayer.HideAllScreens();
            dialogLayer.HideAllScreens();
        }

        /// <summary>
        /// Hide all screens from all layers, except the ones provided.
        /// </summary>
        /// <param name="screenIdToIgnore">Screen ID to be ignored</param>
        public void HideAllScreensExcept(string screenIdToIgnore)
        {
            panelLayer.HideAllScreensExcept(screenIdToIgnore);
            dialogLayer.HideAllScreensExcept(screenIdToIgnore);
        }

        /// <summary>
        /// Hide all screens from all layers, except the ones provided.
        /// </summary>
        /// <param name="screenIdsToIgnore">Array of ScreenIDs to be ignored</param>
        public void HideAllScreensExcept(string[] screenIdsToIgnore)
        {
            panelLayer.HideAllScreensExcept(screenIdsToIgnore);
            dialogLayer.HideAllScreensExcept(screenIdsToIgnore);
        }

        public void HideAllDialogScreens() => dialogLayer.HideAllScreens();

        /// <summary>
        /// Check if the screen with the given screenID is visible.
        /// </summary>
        /// <param name="screenID">The screen id from screen controller.</param>
        /// <returns>true if screen is visible, false if not</returns>
        public bool IsScreenVisible(string screenID)
        {
            if (panelLayer.HasScreen(screenID))
                return panelLayer.IsScreenVisible(screenID);
            else if (dialogLayer.HasScreen(screenID))
                return dialogLayer.IsScreenVisible(screenID);

            return false;
        }

        /// <summary>
        /// Hide the screen specified by screenID.
        /// </summary>
        /// <param name="screenID">The screen id from screen controller.</param>
        public void HideScreen(string screenID)
        {
            if (panelLayer.HasScreen(screenID))
                panelLayer.HideScreen(screenID);
            else if (dialogLayer.HasScreen(screenID))
                dialogLayer.HideScreen(screenID);
        }

        public void RequestScreenDelayed(string screenId, bool showScreen)
        {
            StopAllCoroutines();
            StartCoroutine(RequestScreenDelayedCoroutine(screenId, showScreen));
        }

        private IEnumerator RequestScreenDelayedCoroutine(string screenId, bool showScreen)
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            while (IsScreenVisible(screenId) == showScreen)
            {
                yield return wait;
            }

            yield return wait;

            RequestScreen(screenId, showScreen);
        }
    }
}