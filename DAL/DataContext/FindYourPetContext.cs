namespace DAL.DataContext;

using Model;
using Microsoft.EntityFrameworkCore;

public class FindYourPetContext : DbContext
{
    public FindYourPetContext()
    {
    }

    public FindYourPetContext(DbContextOptions<FindYourPetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(e => e.Id)
                .HasName("users_pk");

            entity.HasIndex(e => e.Login)
                .IsUnique()
                .HasDatabaseName("ulogin");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .IsRequired()
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .IsRequired()
                .HasColumnName("name");

            entity.Property(e => e.Surname)
                .HasMaxLength(128)
                .IsRequired()
                .HasColumnName("surname");

            entity.Property(e => e.Login)
                .HasMaxLength(128)
                .IsRequired()
                .HasColumnName("login");

            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("pets_pk");

            entity.ToTable("pets");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");

            entity.Property(e => e.Age)
                .HasColumnName("age");

            entity.Property(e => e.Description)
                .HasColumnName("description");

            entity.Property(e => e.OwnerId)
                .HasColumnName("owner_id");

            entity.HasOne(p => p.User).WithMany(u => u.Pets)
                .HasForeignKey(p => p.OwnerId)
                .HasConstraintName("fc_user");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("posts_pk");

            entity.ToTable("posts");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");

            entity.Property(e => e.Date)
                .IsRequired()
                .HasColumnName("lost_date");

            entity.Property(e => e.Location)
                .HasMaxLength(128)
                .HasColumnName("location");

            entity.Property(e => e.ContactNumber)
                .HasMaxLength(128)
                .IsRequired()
                .HasColumnName("contact_number");

            entity.Property(e => e.Type)
                .IsRequired()
                .HasColumnName("post_type");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasColumnName("is_active");

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at");

            entity.Property(e => e.PetId)
                .HasColumnName("pet_id");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.HasOne(p => p.Pet).WithMany(p => p.Posts)
                .HasForeignKey(p => p.PetId)
                .HasConstraintName("fc_pet");

            entity.HasOne(p => p.User).WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .HasConstraintName("fc_user");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("image_pk");

            entity.ToTable("images");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");

            entity.Property(e => e.Path)
                .HasMaxLength(256)
                .IsRequired()
                .HasColumnName("path");

            entity.Property(e => e.PetId)
                .HasColumnName("pet_id");

            entity.HasOne(i => i.Pet).WithOne(p => p.Image)
                .HasForeignKey<Image>(i => i.PetId)
                .HasConstraintName("fc_pet");
        });
    }
}