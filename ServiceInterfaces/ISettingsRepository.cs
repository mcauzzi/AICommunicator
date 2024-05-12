using System.Threading.Tasks;

namespace ServiceInterfaces;

public interface ISettingsRepository<TRoot>
{
    public Task Update(TRoot obj);
}