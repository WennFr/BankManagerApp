
//using Windows.UI.Notifications;

//namespace BankManagerApp.Infrastructure.Validation
//{
//    public class NotificationManager
//    {
//        public static void ShowToastNotification(string title, string message)
//        {
//            // Create a new toast notification
//            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
//            var toastTextElements = toastXml.GetElementsByTagName("text");
//            toastTextElements[0].AppendChild(toastXml.CreateTextNode(title));
//            toastTextElements[1].AppendChild(toastXml.CreateTextNode(message));

//            // Set the toast notification duration
//            var toast = new ToastNotification(toastXml);
//            toast.ExpirationTime = DateTime.Now.AddSeconds(10);

//            // Send the toast notification
//            ToastNotificationManager.CreateToastNotifier().Show(toast);
//        }


//    }
//}
