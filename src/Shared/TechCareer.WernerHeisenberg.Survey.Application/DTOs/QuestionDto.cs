using TechCareer.WernerHeisenberg.Survey.Domain.Enums;

namespace TechCareer.WernerHeisenberg.Survey.Application.DTOs;

public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public QuestionTypes QuestionType { get; set; }
    public AnswerTypes AnswerType { get; set; }
    public bool IsPublic { get; set; }
    public bool Deleted { get; set; }

    public IEnumerable<QuestionAnswerDto> Answers { get; set; }
}