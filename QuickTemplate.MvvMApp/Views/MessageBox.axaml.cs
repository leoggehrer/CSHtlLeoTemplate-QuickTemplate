//@BaseCode
//MdStart
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace QuickTemplate.MvvMApp.Views
{
    public partial class MessageBox : Window
    {
        public enum MessageBoxButtons
        {
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel
        }

        public enum MessageBoxResult
        {
            Ok,
            Cancel,
            Yes,
            No
        }

        public MessageBox()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static Task<MessageBoxResult> ShowAsync(Window? parent, string text, string title, MessageBoxButtons buttons)
        {
            var msgbox = new MessageBox()
            {
                Title = title
            };
            msgbox.FindControl<TextBlock>("Text").Text = text;

            var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");
            var result = MessageBoxResult.Ok;

            void AddButton(string caption, MessageBoxResult r, bool def = false)
            {
                var btn = new Button { Content = caption, MinWidth = 100 };

                btn.Click += (_, __) =>
                {
                    result = r;
                    msgbox.Close();
                };
                buttonPanel.Children.Add(btn);
                if (def)
                    result = r;
            }

            if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
                AddButton("Ok", MessageBoxResult.Ok, true);

            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Yes", MessageBoxResult.Yes);
                AddButton("No", MessageBoxResult.No, true);
            }

            if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
                AddButton("Cancel", MessageBoxResult.Cancel, true);

            var tcs = new TaskCompletionSource<MessageBoxResult>();

            msgbox.Closed += delegate { tcs.TrySetResult(result); };
            if (parent != null)
                msgbox.ShowDialog(parent);
            else 
                msgbox.Show();

            return tcs.Task;
        }
    }
}
//MdEnd