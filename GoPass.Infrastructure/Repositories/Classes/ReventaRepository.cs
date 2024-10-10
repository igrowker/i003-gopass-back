using Microsoft.EntityFrameworkCore;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Data;
using GoPass.Infrastructure.Repositories.Interfaces;
using GoPass.Domain.DTOs.Request.PaginationDTOs;
using GoPass.Domain.IQueryableExtensions;
using System.Runtime.CompilerServices;

namespace GoPass.Infrastructure.Repositories.Classes

{
    public class ReventaRepository : GenericRepository<Reventa>, IReventaRepository
    {
        public ReventaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<Reventa> Publish(Reventa reventa, int vendedorId)
        {
            reventa.VendedorId = vendedorId;
            _dbContext.Add(reventa);

            await _dbContext.SaveChangesAsync();

            return reventa;
        }

        public async Task<Reventa> GetResaleByEntradaId(int entradaId)
        {
            var resale = await _dbSet.Where(x => x.EntradaId == entradaId).SingleOrDefaultAsync();

            if (resale is null) throw new Exception();

            return resale;
        }

        public override async Task<List<Reventa>> GetAllWithPagination(PaginationDto paginationDto)
        {
            var recordsQueriable = _dbContext.Set<Reventa>().AsQueryable();

            return await recordsQueriable.Paginate(paginationDto).Include(x => x.Entrada).ToListAsync();
        }

        //public async Task<Reventa> BuyTicket(int resaleId)
        //{
        //    Reventa resale = await _dbSet.FindAsync(resaleId);


        //    if(resale is null) throw new Exception("La reventa no existe.");

        //    var historialCompraVenta = new HistorialCompraVenta
        //    {
        //        GameName = resale.GameName, // Asigna el nombre del juego desde la reventa
        //        Description = resale.Description, // Asigna la descripción desde la reventa
        //        Image = resale.Image, // Asigna la imagen, si está disponible
        //        Address = resale.Address, // Asigna la dirección, si está disponible
        //        EventDate = resale.EventDate, // Asigna la fecha del evento
        //        CodigoQR = resale.CodigoQR, // Asigna el código QR
        //        Verificada = false, // Por defecto, puede ser falsa
        //        EntradaId = resale.EntradaId, // ID de la entrada asociada
        //        VendedorId = resale.VendedorId, // ID del vendedor
        //        CompradorId = resale.CompradorId, // ID del comprador
        //        FechaReventa = DateTime.Now, // La fecha en que se compró la entrada
        //        Precio = resale.Precio, // Precio de la reventa
        //        ResaleDetail = resale.ResaleDetail // Detalles de la reventa
        //    };

        //    _dbContext.Remove(resale);



        //    await _dbContext.SaveChangesAsync();

        //    return reventa;
        //}
    }
}
