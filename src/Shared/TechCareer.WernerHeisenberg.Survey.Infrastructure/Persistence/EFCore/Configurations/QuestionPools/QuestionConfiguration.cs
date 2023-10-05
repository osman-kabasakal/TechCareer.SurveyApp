using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore.Configurations.QuestionPools;

public class QuestionConfiguration:BaseEntityConfiguration<Question>
{
    public override void UpConfigure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");
    }
}