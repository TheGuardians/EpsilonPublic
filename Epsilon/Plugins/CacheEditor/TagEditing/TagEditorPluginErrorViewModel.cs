using Stylet;
using System;

namespace CacheEditor
{
    internal class TagEditorPluginErrorViewModel : Screen
    {
        public string ErrorMessage { get; }

        public TagEditorPluginErrorViewModel(Exception ex)
        {
            ErrorMessage = ex.ToString();
        }
    }
}