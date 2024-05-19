using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Models;
using ServiceInterfaces;

namespace ServiceImplementations;

public class AppSettingsRepository:ISettingsRepository<FrontendConfig>
{
    public async Task Update(FrontendConfig obj)
    {
        var str=JsonSerializer.Serialize(obj,new JsonSerializerOptions(){WriteIndented = true});
        await File.WriteAllTextAsync("appsettings.json", str);
    }
}