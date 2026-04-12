using EventEase.Models;

namespace EventEase.Services
{
    public class EventService : IEventService
    {
        private readonly List<Event> _events = new();
        private int _nextId = 1;

        public Task<List<Event>> GetAllEventsAsync()
        {
            return Task.FromResult(_events.ToList());
        }

        public Task<Event?> GetEventByIdAsync(int id)
        {
            var eventItem = _events.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(eventItem);
        }

        public Task AddEventAsync(Event eventItem)
        {
            eventItem.Id = _nextId++;
            _events.Add(eventItem);
            return Task.CompletedTask;
        }

        public Task UpdateEventAsync(Event eventItem)
        {
            var existing = _events.FirstOrDefault(e => e.Id == eventItem.Id);
            if (existing != null)
            {
                existing.Name = eventItem.Name;
                existing.Location = eventItem.Location;
                existing.Date = eventItem.Date;
            }
            return Task.CompletedTask;
        }

        public Task DeleteEventAsync(int id)
        {
            var eventItem = _events.FirstOrDefault(e => e.Id == id);
            if (eventItem != null)
            {
                _events.Remove(eventItem);
            }
            return Task.CompletedTask;
        }
    }
}