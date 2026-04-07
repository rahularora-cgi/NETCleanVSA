namespace Users.Persistence.Database
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> options, IDomainEventDispatcher domainEventDispatcher): DbContextBase(options, domainEventDispatcher)
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

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
