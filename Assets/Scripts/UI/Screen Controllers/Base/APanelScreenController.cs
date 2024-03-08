using System;
using UnityEngine;

namespace Game.UI
{
    public abstract class APanelScreenController : MonoBehaviour, IPanelScreenController
    {
        //Auto-implemented properties
        public string ScreenID { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = false;

        /// <summary>
        /// Shows the screen without a callback.
        /// </summary>
        /// <param name="values">Optional parameters to send when showing a screen. Each screen must treat them accordingly.</param>
        public virtual void Show(params object[] values)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                IsVisible = true;
            }
        }

        /// <summary>
        /// Show the screen. Can be passed a callback to be executed when the screen is shown.
        /// </summary>
        /// <param name="onCompleteCallback">Action to be passed to screen and invoked after shown.</param>        
        /// <param name="values">Optional parameters to send when showing a screen. Each screen must treat them accordingly.</param> 
        public virtual void Show(Action onCompleteCallback = null, params object[] values)
        {
            Show(values);
            onCompleteCallback?.Invoke();
        }

        /// <summary>
        /// Hides the screen. 
        /// </summary>
        public virtual void Hide()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                IsVisible = false;
            }
        }
    }
}