using Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Final.Data
{
    public class FinalWebContext : DbContext
    {
        public FinalWebContext(DbContextOptions<FinalWebContext> options) : base(options)
        {

        }

        public DbSet<Final.Models.Categoria> Categoria { get; set; } = default!;
        public DbSet<Final.Models.Cliente> Cliente { get; set; } = default!;
        public DbSet<Final.Models.Compra> Compra { get; set; } = default!;
        public DbSet<Final.Models.Localidad> Localidad { get; set; } = default!;
        public DbSet<Final.Models.Provincia> Provincia { get; set; } = default!;
        public DbSet<Final.Models.Marca> Marca { get; set; } = default!;
        public DbSet<Final.Models.Producto> Producto { get; set; } = default!;
        public DbSet<Final.Models.Proveedor> Proveedor { get; set; } = default!;
        public DbSet<Final.Models.Usuario> Usuario { get; set; } = default!;
        public DbSet<Final.Models.Venta> Venta { get; set; } = default!;
        public DbSet<Final.Models.DetalleVenta> DetalleVenta { get; set; } = default!;
        public DbSet<Final.Models.DetalleCompra> DetalleCompra { get; set; } = default!;


    }

}
