using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.SurveyPool;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore.Configurations.SurveyPools;

public class SurveyConfiguration:BaseEntityConfiguration<UserSurvey>
{
    public override void UpConfigure(EntityTypeBuilder<UserSurvey> builder)
    {
        builder.HasOne(x => x.AssignedUser)
            .WithMany(x => x.UserSurveys)
            .HasForeignKey(x => x.AssignedUserId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
    }
}