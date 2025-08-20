using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_FinalProject.Database;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Restaurant_FinalProject.Services
{
    // Service for handling database operations
    // Provides methods for common CRUD operations
    public class DatabaseService
    {
        private readonly RestaurantDbContext _context;
        private bool _isInitialized = false;

        public DatabaseService(RestaurantDbContext context)
        {
            _context = context;
        }

        
        // Initializes the database and seeds sample data if needed
        
        public async Task InitializeAsync()
        {
            if (_isInitialized) return;

            try
            {
                await _context.InitializeDatabaseAsync();
                _isInitialized = true;
                Debug.WriteLine("Database service initialized successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing database service: {ex.Message}");
                throw;
            }
        }

       
        // Gets all entities of a specific type
        
        
        // <returns>List of entities</returns>
        public async Task<List<T>> GetAllAsync<T>() where T : class
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting all {typeof(T).Name}s: {ex.Message}");
                throw;
            }
        }

        
        // Gets an entity by its ID
        
        
        // <returns>The entity or null if not found</returns>
        public async Task<T> GetByIdAsync<T>(int id) where T : class
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting {typeof(T).Name} by ID: {ex.Message}");
                throw;
            }
        }

        
        // Adds a new entity to the database
        
        
        // <returns>True if successful</returns>
        public async Task<bool> AddAsync<T>(T entity) where T : class
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding {typeof(T).Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Updates an existing entity in the database
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity to update</param>
        /// <returns>True if successful</returns>
        public async Task<bool> UpdateAsync<T>(T entity) where T : class
        {
            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating {typeof(T).Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes an entity from the database
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="id">ID of entity to delete</param>
        /// <returns>True if successful</returns>
        public async Task<bool> DeleteAsync<T>(int id) where T : class
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<T>().Remove(entity);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting {typeof(T).Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Executes a custom query against the database
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">Query function</param>
        /// <returns>Query results</returns>
        public async Task<List<T>> QueryAsync<T>(Func<IQueryable<T>, IQueryable<T>> query) where T : class
        {
            try
            {
                var result = query(_context.Set<T>());
                return await result.ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error executing query for {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the database file path (for debugging purposes)
        /// </summary>
        /// <returns>Database file path</returns>
        public string GetDatabasePath()
        {
            return Path.Combine(FileSystem.AppDataDirectory, "restaurant.db");
        }
    }
}
