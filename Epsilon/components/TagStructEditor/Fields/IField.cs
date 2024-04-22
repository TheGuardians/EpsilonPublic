using EpsilonLib.Menus;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TagStructEditor.Common;

namespace TagStructEditor.Fields
{
    public abstract class IField : PropertyChangedNotifier, IDisposable
    {
        /// <summary>
        /// The Parent <see cref="IField" /> this field
        /// </summary>
        public IField Parent { get; set; }

        /// <summary>
        /// Populates the field recursively
        /// </summary>
        /// <param name="owner">The owner of used to get the field's value</param>
        /// <param name="value">The value to set. If null it will get the value from the Owner</param>
        public abstract void Populate(object owner, object value = null);

        /// <summary>
        /// Visitor Accept function - Allows functionality to be implemented without having to modify the tag field interface.
        /// </summary>
        /// <param name="visitor">The visitor</param>
        public abstract void Accept(IFieldVisitor visitor);

        /// <summary>
        /// Called when the context menu should be populated
        /// </summary>
        /// <param name="menu"></param>
        protected virtual void OnPopulateContextMenu(EMenu menu) { }

        /// <summary>
        /// Populate the context menu for this field and its parents.
        /// </summary>
        /// <param name="menu"></param>
        public void PopulateContextMenu(EMenu menu)
        {
            //var oldItemCount = menu.Items.Count;
            OnPopulateContextMenu(menu);

            //if (menu.Items.Count > oldItemCount && Parent != null)
            //    menu.Items.Add(new Separator());

            //if (Parent != null)
            //    Parent.PopulateContextMenu(menu);
        }

        public virtual void Dispose() { }
    }
}
