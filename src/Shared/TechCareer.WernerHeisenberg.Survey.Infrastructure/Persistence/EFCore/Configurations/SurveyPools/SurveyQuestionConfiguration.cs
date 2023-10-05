using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.SurveyPool;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore.Configurations.SurveyPools;

public class SurveyQuestionConfiguration: BaseEntityConfiguration<SurveyQuestion>
{
    public override void UpConfigure(EntityTypeBuilder<SurveyQuestion> builder)
    {
        builder.HasOne(x => x.Question)
            .WithMany(x => x.SurveyQuestions)
            .HasForeignKey(x => x.QuestionId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(x => x.Survey)
            .WithMany(x => x.SurveyQuestions)
            .HasForeignKey(x => x.SurveyId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(x => x.CorrectAnswer)
            .WithMany(x => x.ChosenBySurveyQuestions)
            .HasForeignKey(x => x.CorrectAnswerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}