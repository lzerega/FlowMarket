using Orders.Api.Models;
using Orders.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Orders.Api.Repositories;

public class OrderRepository
{
    private readonly OrdersDbContext _context;

    public OrderRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.OrdersDb
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.OrdersDb
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Order> AddAsync(Order objeto)
    {
        var entidad = await _context.OrdersDb.AddAsync(objeto);
        await _context.SaveChangesAsync();
        return entidad.Entity;
    }
}