using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EpsilonLib.Editors
{
    public interface IEditorService
    {
        IEnumerable<IEditorProvider> EditorProviders { get; }

        Task OpenFileWithEditorAsync(string filePath, Guid editorProviderId);
        Task OpenFileAsync(string filePath);
    }
}
