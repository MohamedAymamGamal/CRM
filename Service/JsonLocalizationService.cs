using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.API.Service
{
    public class JsonLocalizationService
    {
          private readonly Dictionary<string, Dictionary<string, string>> _languages;
    private readonly IHttpContextAccessor _context;

    public JsonLocalizationService(IHttpContextAccessor context)
    {
        _context = context;
        _languages = LoadLanguages();
    }

    private Dictionary<string, Dictionary<string, string>> LoadLanguages()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Localization");

        return Directory
            .GetFiles(path, "*.json")
            .ToDictionary(
                file => Path.GetFileNameWithoutExtension(file),
                file => JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(file))
            );
    }

    public string T(string key)
    {
        var lang = _context.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? "en";

        if (!_languages.ContainsKey(lang)) lang = "en";

        return _languages[lang].ContainsKey(key)
            ? _languages[lang][key]
            : key;
    }
    }
}