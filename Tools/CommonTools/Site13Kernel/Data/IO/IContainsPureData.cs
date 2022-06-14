namespace Site13Kernel.Data.IO
{
    public interface IContainsPureData
    {
        IPureData ObtainData();
        void ApplyData(IPureData data);
    }
}
