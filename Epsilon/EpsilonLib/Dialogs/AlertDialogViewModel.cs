using System.Windows.Media;
using Stylet;

namespace EpsilonLib.Dialogs
{
    public class AlertDialogViewModel : Screen
    {
        private string _message;
        private string _subMessage = "Click OK to continue.";
        private Geometry _icon;
        private Brush _iconColor = Brushes.RoyalBlue;
        private Alert _alertType = Alert.Standard;
        private bool _cancelVisible = false;

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

        public Geometry Icon
        {
            get => _icon;
            set => SetAndNotify(ref _icon, value);
        }

        public bool CancelVisible
        {
            get => _cancelVisible;
            set => SetAndNotify(ref _cancelVisible, value);
        }

        public Alert AlertType
        {
            get => _alertType;
            set
            {
                SetAndNotify(ref _alertType, value);
                switch(value)
                {
                    case Alert.Warning:
                        {
                            CancelVisible = true;
                            DisplayName = "Warning";
                            IconColor = Brushes.Goldenrod;
                            Icon = Geometry.Parse("M12 5.99L19.53 19H4.47L12 5.99M12 2L1 21h22L12 2zm1 14h-2v2h2v-2zm0-6h-2v4h2v-4z");
                        }
                        break;
                    case Alert.Error:
                        {
                            CancelVisible = false;
                            DisplayName = "Error";
                            IconColor = Brushes.Red;
                            Icon = Geometry.Parse("M19,19H5V5h14V19z M3,3v18h18V3H3z M17,15.59L15.59,17L12,13.41L8.41,17L7,15.59L10.59,12L7,8.41L8.41,7L12,10.59L15.59,7 L17,8.41L13.41,12L17,15.59z");
                        }
                        break;
                    case Alert.Success:
                        {
                            CancelVisible = false;
                            DisplayName = "Success";
                            IconColor = Brushes.LimeGreen;
                            Icon = Geometry.Parse("M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8zm4.59-12.42L10 14.17l-2.59-2.58L6 13l4 4 8-8z");
                        }
                        break;
                    default:
                        {
                            DisplayName = "Alert";
                            Icon = Geometry.Parse("M11 7h2v2h-2zm0 4h2v6h-2zm1-9C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8z");
                        }
                        break;
                }
            }
        }

        public Brush IconColor
        {
            get => _iconColor;
            set => SetAndNotify(ref _iconColor, value);
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

    public enum Alert
    {
        Standard,
        Warning,
        Error,
        Success
    }
}
