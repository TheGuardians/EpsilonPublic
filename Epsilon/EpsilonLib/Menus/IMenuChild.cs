namespace EpsilonLib.Menus
{
    public interface IMenuChild
    {
        object Parent { get; }
        object Group { get; }
        IMenuChild PlaceAfterChild { get; }
    }
}
