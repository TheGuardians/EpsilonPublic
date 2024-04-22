using EpsilonLib.Commands;

namespace MapVariantFixer
{
    [ExportCommand]
    class ShowMapVariantFixerCommand : CommandDefinition
    {
        public override string Name => "MapVariantFixer.Show";

        public override string DisplayText => "Update Map Variants";
    }
}
