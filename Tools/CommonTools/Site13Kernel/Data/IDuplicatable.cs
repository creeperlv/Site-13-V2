namespace Site13Kernel.Data
{
    public interface IDuplicatable
    {
        IDuplicatable Duplicate();
    }
    public interface IDuplicatable<T>
    {
        T Duplicate();
    }
}
