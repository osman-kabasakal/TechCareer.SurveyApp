using System.ComponentModel.DataAnnotations;

namespace TechCareer.WernerHeisenberg.Survey.Domain.Enums;

public enum AnswerTypes
{
    [Display(Name = "Tek şıklı")]
    Singular,
    [Display(Name = "Çoktan seçmeli")]
    Multiple,
}