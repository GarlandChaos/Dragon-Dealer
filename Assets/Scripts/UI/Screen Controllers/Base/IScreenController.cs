using System;

namespace Game.UI
{
    public interface IScreenController
    {
        public string ScreenID { get; set; }
        public bool IsVisible { get; set; }

        void Show(params object[] values);
        void Show(Action onCompleteCallback = null, params object[] values);
        void Hide();
    }

    public interface IPanelScreenController : IScreenController { }

    public interface IDialogScreenController : IScreenController
    {
        public DialogLayer DialogLayer { set; }
    }
}