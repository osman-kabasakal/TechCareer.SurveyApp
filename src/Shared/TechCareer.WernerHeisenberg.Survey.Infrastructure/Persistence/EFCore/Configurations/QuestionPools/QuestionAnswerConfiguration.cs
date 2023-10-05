using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore.Configurations.QuestionPools;

public class QuestionAnswerConfiguration:BaseEntityConfiguration<QuestionAnswer>
{
    public override void UpConfigure(EntityTypeBuilder<QuestionAnswer> builder)
    {
        builder.HasOne(x => x.Question)
            .WithMany(x => x.Answers)
            .HasForeignKey(x => x.QuestionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}