using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace BlamScriptEditorPlugin
{
    /// <summary>
    /// Interaction logic for ScriptTagEditorView.xaml
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ScriptTagEditorView : UserControl
    {
        public ScriptTagEditorView()
        {
            InitializeComponent();
        }

        private void ScriptKeyDownHandler(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Enter:
                    InsertText("\r\n"); e.Handled = true;
                    break;
                case Key.Tab:
                    InsertText("\t"); e.Handled = true;
                    break;
                case Key.Z:
                    if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
                        ScriptSourceTextBox.Undo();
                    break;
                default:
                    break;
            }
        }

        private void InsertText(string insert)
        {
            var cursorPosition = ScriptSourceTextBox.SelectionStart;
            ScriptSourceTextBox.Text = ScriptSourceTextBox.Text.Insert(ScriptSourceTextBox.SelectionStart, insert);
            ScriptSourceTextBox.SelectionStart = cursorPosition + insert.Length;
        }

        private void ScriptSourceTextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                var cursorPosition = ScriptSourceTextBox.SelectionStart;
                string toPaste = Clipboard.GetText();
                ScriptSourceTextBox.Text = ScriptSourceTextBox.Text.Insert(cursorPosition, (toPaste));
                ScriptSourceTextBox.SelectionStart = cursorPosition + toPaste.Length;

                e.Handled = true;
            }
        }
    }
}
