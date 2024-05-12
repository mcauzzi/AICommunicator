using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Models;
using ServiceImplementations.Configs;
using ServiceInterfaces;

namespace ServiceImplementations;

public class AppSettingsRepository:ISettingsRepository<CommConfig>
{
    public async Task Update(CommConfig obj)
    {
        var str=JsonSerializer.Serialize(obj);
        await File.WriteAllTextAsync("appsettings.json", str);
    }
}