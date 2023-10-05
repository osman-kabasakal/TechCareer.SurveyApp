using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.SurveyPool;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore.Configurations.SurveyPools;

public class SolverAnswerConfiguration: BaseEntityConfiguration<SolverAnswer>
{
    public override void UpConfigure(EntityTypeBuilder<SolverAnswer> builder)
    {
        builder.HasOne(x => x.SurveyQuestion)
            .WithMany(x => x.SolverAnswers)
            .HasForeignKey(x => x.SurveyQuestionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.SolverUser)
            .WithMany(x => x.SolverAnswers)
            .HasForeignKey(x => x.SolverUserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(x => x.SelectedAnswer)
            .WithMany(x => x.PreferredSolventsAnswers)
            .HasForeignKey(x => x.SelectedAnswerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}