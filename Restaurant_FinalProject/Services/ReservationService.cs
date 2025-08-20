using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_FinalProject.Data;
using Restaurant_FinalProject.Database;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace Restaurant_FinalProject.Services
{
    public class ReservationService
    {
        private readonly RestaurantDbContext _context;

        public ReservationService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .FirstOrDefaultAsync(r => r.ReservationID == id);
        }

        public async Task<bool> AddReservationAsync(Reservation reservation)
        {
            try
            {
                await _context.Reservations.AddAsync(reservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding reservation: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateReservationAsync(Reservation reservation)
        {
            try
            {
                _context.Reservations.Update(reservation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating reservation: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation != null)
                {
                    _context.Reservations.Remove(reservation);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting reservation: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Reservation>> GetReservationsByDateAsync(DateTime date)
        {
            return await _context.Reservations
                .Where(r => r.ReservationDate.Date == date.Date)
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetUpcomingReservationsAsync()
        {
            return await _context.Reservations
                .Where(r => r.ReservationDate >= DateTime.Now)
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
        }
    }
}
