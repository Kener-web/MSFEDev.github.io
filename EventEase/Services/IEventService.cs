using EventEase.Models;

namespace EventEase.Services
{
    public interface IEventService
    {
        Task<List<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task AddEventAsync(Event eventItem);
        Task UpdateEventAsync(Event eventItem);
        Task DeleteEventAsync(int id);
    }
}