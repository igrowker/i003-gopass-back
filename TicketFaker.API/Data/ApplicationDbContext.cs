using Microsoft.EntityFrameworkCore;
using TicketFaker.API.Models;
using static System.Net.WebRequestMethods;

namespace TicketFaker.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
               
        }

        public DbSet<Ticket> Tickets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeding de datos
            modelBuilder.Entity<Ticket>().HasData(GenerateTickets(100));
        }

        private Ticket[] GenerateTickets(int count)
        {
            var tickets = new Ticket[count];
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                tickets[i] = new Ticket
                {
                    Id = i + 1,
                    GameName = $"Partido {i + 1}: Equipo A vs Equipo B",
                    Description = $"Descripción del partido {i + 1}",
                    Image = "https://media.airedesantafe.com.ar/p/1104be57d6bbde5daa49a8111ee3c158/adjuntos/268/imagenes/003/840/0003840291/1200x0/smart/argentina-espanapng.png", // o puedes agregar imágenes ficticias si es necesario
                    Address = $"Dirección {i + 1}, Ciudad {random.Next(1, 100)}",
                    EventDate = DateTime.UtcNow.AddDays(random.Next(1, 365)),
                    CodigoQR = Guid.NewGuid().ToString(),
                    Puerta = $"Puerta {random.Next(1, 10)}",
                    Fila = $"Fila {random.Next(1, 50)}",
                    Asiento = $"Asiento {random.Next(1, 100)}"
                };
            }

            return tickets;
        }
    }
}
