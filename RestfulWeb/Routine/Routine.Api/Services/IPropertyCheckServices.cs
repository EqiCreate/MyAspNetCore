namespace Routine.Api.Services
{
    public interface IPropertyCheckServices
    {
        bool TypeHasProPerty<T>(string fields);
    }
}