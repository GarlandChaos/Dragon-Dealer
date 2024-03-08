using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class DialogLayer : ALayer<IDialogScreenController>
    {
        //Object data
        IDialogScreenController currentScreen = null;

        /// <summary>
        /// Registers a screen with given screenID and screen controller and set the transform parent to this layer's GameObject.
        /// </summary>
        /// <param name="screenID">The screen id from screen controller.</param>
        /// <param name="screenController">The screen controller which implements interface of type IScreenController. </param>
        /// <param name="screenTransform">Transform from screen controller.</param>
        public override void RegisterScreen(string screenID, IDialogScreenController screenController, Transform screenTransform)
        {
            if (!screensDictionary.ContainsKey(screenID))
            {
                screenController.ScreenID = screenID;
                screenController.DialogLayer = this;
                screensDictionary.Add(screenID, screenController);
                screenTransform.SetParent(transform, false);
            }
        }

        /// <summary>
        /// Show screen registered in this layer.
        /// </summary>
        /// <param name="screenID">The screen id from screen controller.</param>        
        /// <param name="showCallback">Action to be passed to screen and invoked after shown.</param>        
        /// <param name="values">Optional parameters to send when showing a screen. Each screen must treat them accordingly.</param>
        public override void ShowScreen(string screenID, Action showCallback = null, params object[] values)
        {
            if (screensDictionary.ContainsKey(screenID))
            {
                if (currentScreen != null && currentScreen != screensDictionary[screenID])
                    currentScreen.Hide();

                currentScreen = screensDictionary[screenID];
                currentScreen.Show(showCallback, values);
            }
        }

        /// <summary>
        /// Hide screen registered in this layer.
        /// </summary>
        /// <param name="screenID">The screen id from screen controller.</param>
        public override void HideScreen(string screenID)
        {
            if (currentScreen != null)
            {
                currentScreen.Hide();
                currentScreen = null;
            }

            if (screensDictionary.ContainsKey(screenID))
                screensDictionary[screenID].Hide();
        }

        /// <summary>
        /// Hides all screens registered in this layer.
        /// </summary>
        public override void HideAllScreens()
        {
            foreach (KeyValuePair<string, IDialogScreenController> screen in screensDictionary)
                screen.Value.Hide();
        }
    }
}