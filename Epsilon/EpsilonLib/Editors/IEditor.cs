using Shared;
using Stylet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EpsilonLib.Editors
{
    public interface IEditorProvider
    {
        string DisplayName { get; }

        Guid Id { get; }

        IReadOnlyList<string> FileExtensions { get; }

        Task OpenFileAsync(IShell shell, string fileName);
    }

    public interface IEditor : IScreen
    {

    }
}
