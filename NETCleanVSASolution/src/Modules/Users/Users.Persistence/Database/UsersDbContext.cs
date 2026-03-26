namespace Users.Persistence.Database
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> options): DbContextBase(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
             .ToTable("Users");

            modelBuilder.Entity<Role>()
             .ToTable("Roles");

            modelBuilder.Entity<UserRole>()
             .ToTable("UserRoles");

        }
    }
}
