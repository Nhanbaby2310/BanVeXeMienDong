using BanVeXeMienDong.Models;

namespace BanVeXeMienDong.Repositories
{
    public interface ITicketRepository
    {
        List<Ticket> GetAll();

        void Add(Ticket ticket);

        void Update(Ticket ticket);

        void Remove(Ticket ticket);
    }
}