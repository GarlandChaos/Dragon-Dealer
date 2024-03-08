using System;
using UnityEngine;

namespace Game.UI
{
    public abstract class ADialogScreenController : MonoBehaviour, IDialogScreenController
    {
        //Auto-implemented properties
        public string ScreenID { get; set; } = string.Empty; //Screen ID, uses the game object name
        public bool IsVisible { get; set; } = false; //Indicates if screen is visible or not
        public DialogLayer DialogLayer { protected get; set; } = null; //Dialog layer this screen is registered to

        /// <summary>
        /// Shows the screen without a callback.
        /// </summary>
        /// <param name="values">Optional parameters to send when showing a screen. Each screen must treat them accordingly.</param>
        public virtual void Show(params object[] values)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                gameObject.transform.SetAsLastSibling();
                IsVisible = true;
            }
        }

        /// <summary>
        /// Show the screen.
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