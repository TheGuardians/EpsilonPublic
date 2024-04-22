using System.Collections.Generic;

namespace EpsilonLib.Options
{
    public interface IOptionsService
    {
        IEnumerable<IOptionsPage> OptionPages { get; }
    }
}
