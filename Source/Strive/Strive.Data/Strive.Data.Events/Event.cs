
namespace Strive.Data.Events
{
    /// <summary>
    /// Events are mutable objects for ease of construction.
    /// Every Event has a description.
    /// Event subclasses contain all the changes that will be applied as a transaction.
    /// </summary>
    public class Event
    {
        public string Description { get; set; }
    }
}
