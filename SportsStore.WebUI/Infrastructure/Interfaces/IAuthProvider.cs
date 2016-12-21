namespace SportsStore.WebUI.Infrastructure.Interfaces
{
    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
    }
}