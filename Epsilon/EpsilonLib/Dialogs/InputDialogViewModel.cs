using Stylet;

namespace EpsilonLib.Dialogs
{
    public class InputDialogViewModel : Screen
    {
        private string _inputText;
        private string _message;
        private string _subMessage;

        public string InputText
        {
            get => _inputText;
            set => SetAndNotify(ref _inputText, value);
        }

        public string Message
        {
            get => _message;
            set => SetAndNotify(ref _message, value);
        }

        public string SubMessage
        {
            get => _subMessage;
            set => SetAndNotify(ref _subMessage, value);
        }

        public void Confirm()
        {
            RequestClose(true);
        }

        public void Cancel()
        {
            RequestClose(false);
        }
    }
}
