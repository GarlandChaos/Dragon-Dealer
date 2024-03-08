using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI
{
    public abstract class ALayer<T> : MonoBehaviour where T : IScreenController
    {
        //Object data
        protected Dictionary<string, T> screensDictionary = new Dictionary<string, T>();

        /// <summary>
        /// Registers a screen with given screenID and screen controller and set the transform parent to this layer's GameObject.
        /// </summary>
        /// <param name="screenID">The screen id from screen controller.</param>
        /// <param name="screenController">The screen controller which implements interface of type IScreenController. </param>
        /// <param name="screenTransform">Transform from screen controller.</param>
        public virtual void RegisterScreen(string screenID, T screenController, Transform screenTransform)
        {
            if (!screensDictionary.ContainsKey(screenID))
            {
                screenController.ScreenID = screenID;
                screensDictionary.Add(screenID, screenController);
                screenTransform.SetParent(transform, false);
            }
        }

        /// <summary>
        /// Unregister screen from the layer.
        /// </summary>
        /// <param name="screenID">The screen id from screen controller.</param>
        public virtual void UnregisterScreen(string screenID)
        {
            if (screensDictionary.ContainsKey(screenID))
                screensDictionary.Remove(screenID);
        }

        public virtual bool TryToGetScreenById(string screenID, out T screenController)
        {
            if (HasScreen(screenID))
            {
                screenController = screensDictionary[screenID];
                return true;
            }
            else
            {
                screenController = default(T);
                return false;
            }
        }

        /// <summary>
        /// Show screen registered in this layer.
        /// </summary>
        /// <param name="screenID">The screen id from screen controller.</param>
        /// <param name="showCallback">Action to be passed to screen and invoked after shown.</param>
        /// <param name="values">Optional parameters to send when showing a screen. Each screen must treat them accordingly.</param> 
        public virtual void ShowScreen(string screenID, Action showCallback = null, params object[] values)
        {
            screensDictionary[screenID].Show(showCallback, values);
        }

        /// <summary>
        /// Hide screen registered in this layer.
        /// </summary>
        /// <param name="screenID">The screen id from screen controller.</param>
        public virtual void HideScreen(string screenID)
        {
            screensDictionary[screenID].Hide();
        }

        /// <summary>
        /// Hides all screens registered in this layer.
        /// </summary>
        public virtual void HideAllScreens()
        {
            foreach (KeyValuePair<string, T> kvp in screensDictionary)
                kvp.Value.Hide();
        }

        /// <summary>
        /// Hides all screens registered in this layer, except the ones provided
        /// </summary>
        /// <param name="screenIdToIgnore">Screen ID to be ignored</param>
        public virtual void HideAllScreensExcept(string screenIdToIgnore)
        {
            foreach (KeyValuePair<string, T> kvp in screensDictionary)
            {
                if (kvp.Key.Equals(screenIdToIgnore)) //current screenId key equals screenIdToIgnore
                    continue; //ignore
                else
                    kvp.Value.Hide();
            }
        }

        /// <summary>
        /// Hides all screens registered in this layer, except the ones provided
        /// </summary>
        /// <param name="screenIdsToIgnore">Array of ScreenIDs to be ignored</param>
        public virtual void HideAllScreensExcept(string[] screenIdsToIgnore)
        {
            foreach (KeyValuePair<string, T> kvp in screensDictionary)
            {
                if (screenIdsToIgnore.Any(arrayId => arrayId.Equals(kvp.Key))) //current screenId key is present on ingore array
                    continue; //ignore
                else
                    kvp.Value.Hide();
            }
        }

        /// <summary>
        /// Check if layer contains the screen from screen id.
        /// </summary>
        /// <param name="screenID">The screen id from screen controller.</param>
        /// <returns>true if layer contains the screen, false if not</returns>
        public virtual bool HasScreen(string screenID)
        {
            if (screensDictionary.ContainsKey(screenID))
                return true;

            return false;
        }

        /// <summary>
        /// Check if screen is visible.
        /// </summary>
        /// <param name="screenID">The screen id from screen controller.</param>
        /// <returns>true if visible, false if not</returns>
        public virtual bool IsScreenVisible(string screenID)
        {
            return screensDictionary[screenID].IsVisible;
        }
    }
}