namespace Auth.Core.Interfaces
{
    public interface IHashing
    {
        public string Generate(string password);
    }
}
