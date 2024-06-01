namespace PharmaPro.Core.Contract.Identity
{
    public interface IOTPGenerator
    {
        string Generate(int length);
    }
}