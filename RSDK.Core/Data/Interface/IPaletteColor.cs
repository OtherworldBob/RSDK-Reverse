namespace RSDK.Core.Data.Interface
{
    public interface IPaletteColor : IRsdkDataPersist
    {
        byte R { get; set; }
        byte G { get; set; }
        byte B { get; set; }
    }
}
