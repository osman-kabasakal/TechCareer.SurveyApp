using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore.Configurations;

public abstract class BaseEntityConfiguration<TEntity>: IEntityTypeConfiguration<TEntity>
where TEntity : class, IEntity, new()
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(typeof(TEntity).Name + "s");
        builder.Property(x => x.TimeStamp).IsRowVersion();
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasKey(x => x.Id);

        if (typeof(TEntity).IsAssignableTo(typeof(IAuditableEntity)))
        {
            builder.Property(x=>((IAuditableEntity)x).CreateDate).HasDefaultValueSql("getdate()");
            builder.Property(x=>((IAuditableEntity)x).ModifyDate).HasDefaultValueSql("getdate()");
            
            
        }

        if (typeof(TEntity).GetInterfaces().Any(x=>x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IAuditableEntity<>)))
        {
            builder.Property<int>(nameof(IAuditableEntity<object>.CreatorUserId)).IsRequired();
            builder.HasOne(nameof(IAuditableEntity<object>.CreatorUser))
                .WithMany()
                .HasForeignKey(nameof(IAuditableEntity<object>.CreatorUserId))
                .IsRequired().OnDelete(DeleteBehavior.NoAction);
            
            builder.Property<int?>(nameof(IAuditableEntity<object>.ModifierUserId)).IsRequired(false);
            builder.HasOne(nameof(IAuditableEntity<object>.ModifierUser))
                .WithMany()
                .HasForeignKey(nameof(IAuditableEntity<object>.ModifierUserId))
                .IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        }
        
        if (typeof(TEntity).IsAssignableTo(typeof(ISoftDeleteEntity)))
        {
            builder.Property(x=>((ISoftDeleteEntity)x).Deleted).HasDefaultValue(false);
        }
        
        
        UpConfigure(builder);
        // builder.HasData(SeedData.Seder.GetSeedData<TEntity>());
    }

    public abstract void UpConfigure(EntityTypeBuilder<TEntity> builder);


}