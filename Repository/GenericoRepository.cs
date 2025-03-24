using InventoryFinal.Data;
using InventoryFinal.EscribirLogsFichero;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InventoryFinal.Repository
{
    public class GenericoRepository<T> : IGenericoRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        // Propiedad protegida que nos permite acceder a las entidades de la base de datos
        protected DbSet<T> Entidades => _context.Set<T>();

        public GenericoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> Insert(T entity)
        {
            try
            {
                EntityEntry<T> resultado = await Entidades.AddAsync(entity);
                return resultado.Entity;
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al insertar la entidad a la BD: " + ex.Message);
                throw;
            }
        }

        public async Task<T> GetById(int id)
        {
            try
            {
                var entidad = await Entidades.FindAsync(id);
                return entidad;
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al obtener la entidad de la BD: " + ex.Message);
                throw;
            }
        }

        public async Task<List<T>> GetAll()
        {
            try
            {
                List<T> resultado = await Entidades.ToListAsync();
                return resultado;
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al obtener las entidades de la BD: " + ex.Message);
                throw;
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                Entidades.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al actualizar la entidad de la BD: " + ex.Message);
                return false;
                
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var entidad = Entidades.Find(id);

                if (entidad != null)
                {
                    Entidades.Remove(entidad);

                    await _context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al borrar la entidad de la BD: " + ex.Message);
                throw;
            }
        }
    }
}