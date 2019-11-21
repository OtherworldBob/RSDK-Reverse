using RSDK.Core.IO;

namespace RSDK.Core.Data.Interface
{
    public interface IRsdkDataPersist
    {
        void Read(RsdkReader reader);
        void Write(RsdkWriter writer);
    }
}
