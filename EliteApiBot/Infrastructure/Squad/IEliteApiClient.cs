using EliteApiBot.Model;

namespace EliteApiBot.Infrastructure.Squad
{
    public interface IEliteApiClient
    {
        public Task<IReadOnlyCollection<SquadInfo>> GetSquadInfoAsync(string tag);
        public Task<IReadOnlyCollection<FullSquadInfo>> GetFullSquadInfoAsync(string tag);
        public Task<Model.Player?> GetPlayerAsync(string name);
    }
}
