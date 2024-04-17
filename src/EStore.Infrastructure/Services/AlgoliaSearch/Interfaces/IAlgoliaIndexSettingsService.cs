using Algolia.Search.Models.Settings;

namespace EStore.Infrastructure.Services.AlgoliaSearch.Interfaces;

public interface IAlgoliaIndexSettingsService
{
    Task<IndexSettings> GetIndexSettingsAsync();
}
