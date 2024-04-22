using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Xml;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Media;
using TagTool.Scripting;
using TagTool.Cache;

namespace BlamScriptEditorPlugin
{
    class ScriptTextEditor : TextEditor
    {
        private static IHighlightingDefinition SyntaxHighlightingDefinition;

        public ScriptTextEditor()
        {
            if(SyntaxHighlightingDefinition == null)
            {
                var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (Stream s = File.OpenRead(Path.Combine(dir, "Resources/HaloScript.xshd")))
                {
                    using (XmlTextReader reader = new XmlTextReader(s))
                    {
                        var xshd = HighlightingLoader.LoadXshd(reader);

                        SyntaxHighlightingDefinition = HighlightingLoader.Load(xshd, HighlightingManager.Instance);


                        //
                        //var rules = SyntaxHighlightingDefinition.MainRuleSet.Rules;
                        //rules.Add(new HighlightingRule() { Regex = new Regex("\\b\\(sleep\\s+"), Color = SyntaxHighlightingDefinition.GetNamedColor("Declaration") });

                        //var regexItem = new Regex("^[a-z_0-9]+");
                        //var scripts = ScriptInfo.Scripts[(CacheVersion.HaloOnlineED, CachePlatform.Original)];
                        //var keywords = string.Join("|", scripts.Values.Select(x => x.Name).Where(x => regexItem.IsMatch(x)).ToArray());

                        //var span = new HighlightingSpan()
                        //{
                        //    StartExpression = new Regex($"\\((?=({keywords}))"),
                        //    EndExpression = new Regex("\\s"),
                        //    RuleSet = new HighlightingRuleSet()
                        //};
                        //span.RuleSet.Rules.Add(new HighlightingRule()
                        //{
                        //    Regex = new Regex($"({keywords})"),
                        //    Color = SyntaxHighlightingDefinition.GetNamedColor("Declaration")
                        //});

                    }
                }
            }

            this.SyntaxHighlighting = SyntaxHighlightingDefinition;
        }

        /// <summary>
        /// A bindable Text property
        /// </summary>
        public new string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
                RaisePropertyChanged("Text");
            }
        }

        /// <summary>
        /// The bindable text property dependency property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(ScriptTextEditor),
                new FrameworkPropertyMetadata
                {
                    DefaultValue = default(string),
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = OnDependencyPropertyChanged
                }
            );

        protected static void OnDependencyPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = (ScriptTextEditor)obj;

            if (target.Document != null)
            {
                var caretOffset = target.CaretOffset;
                var newValue = args.NewValue;

                if (newValue == null)
                {
                    newValue = "";
                }

                target.Document.Text = (string)newValue;
                target.CaretOffset = Math.Min(caretOffset, newValue.ToString().Length);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (this.Document != null)
            {
                Text = this.Document.Text;
            }

            base.OnTextChanged(e);
        }

        /// <summary>
        /// Raises a property changed event
        /// </summary>
        /// <param name="property">The name of the property that updates</param>
        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
