using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TechCareer.WernerHeisenberg.Survey.Core.ApplicationExtesions;
using TechCareer.WernerHeisenberg.Survey.Core.AppSettings;
using TechCareer.WernerHeisenberg.Survey.Core.Constants.Authorizes;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;
using TechCareer.WernerHeisenberg.Survey.Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore;

public class ContextSeedService: IDbContextSeedService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IApplicationDbContext _context;
    private readonly IOptions<SeedSetting> _seedSettingOptions;

    public ContextSeedService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IApplicationDbContext context,
        IOptions<SeedSetting> seedSettingOptions)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _seedSettingOptions = seedSettingOptions;
    }
    
    public async Task ContextSeedAsync(CancellationToken cancellationToken = default)
    {
        if (_userManager.Users.Any(x => x.Email != null && x.Email.Equals(_seedSettingOptions.Value.Email)))
            return;

        var user = new ApplicationUser
        {
            UserName = _seedSettingOptions.Value.Email,
            Email = _seedSettingOptions.Value.Email,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            Firstname = _seedSettingOptions.Value.Firstname,
            Lastname = _seedSettingOptions.Value.Lastname,
            SystemUser= true
        };
        
        await _userManager.CreateAsync(user, _seedSettingOptions.Value.Password);
        
        var roleDev = new ApplicationRole
        {
            Name = RoleNamesConstants.Devoloper,
            Creator = user.Id,
            CreateDate = DateTime.Now.ToIstanbulDateTime(),
        };  
        await _roleManager.CreateAsync(roleDev);
        
        await _userManager.AddToRoleAsync(user, roleDev.Name);


        var roleAdmin = new ApplicationRole
        {
            Name = RoleNamesConstants.Admin,
            Creator = user.Id,
            CreateDate = DateTime.Now.ToIstanbulDateTime(),
            SystemRole= true
        };  
        
        await _roleManager.CreateAsync(roleAdmin);
        
        var roleRegistered = new ApplicationRole
        {
            Name = RoleNamesConstants.RegisteredUser,
            Creator = user.Id,
            CreateDate = DateTime.Now.ToIstanbulDateTime(),
            SystemRole= true
        };  
        
        await _roleManager.CreateAsync(roleRegistered);
        
        var roleNotRegistered = new ApplicationRole
        {
            Name = RoleNamesConstants.NotRegisteredUser,
            Creator = user.Id,
            CreateDate = DateTime.Now.ToIstanbulDateTime(),
            SystemRole= true
        };  
        
        await _roleManager.CreateAsync(roleNotRegistered);
        
       await _context.RunInTransaction(async () =>
        {
            await _context.SaveChangesAsync(cancellationToken);
            
             
            var questions = GetDefaultQuestions(user);
            await (_context as DbContext)?.Set<Question>().AddRangeAsync(questions, cancellationToken)!;
            await _context.SaveChangesAsync(cancellationToken);
        }, cancellationToken);
       
        
    }

    private List<Question> GetDefaultQuestions(ApplicationUser user)
    {
        return new List<Question>()
        {
            GetQuestion("En sevdiği yemek?",user, 
                "Patates kızartması",
                "Burger",
                "Döner",
                "Kuru Fasulye",
                "Makarna"
                ),
            GetQuestion(
                "En sevdiği müzik türü?",
                user,
                "Pop",
                "Rap",
                "Rock",
                "Türk halk müziği",
                "Arabask"
                ),
            GetQuestion(
                "Zamanını nasıl geçirir?",
                user,
                "Uyuyarak",
                "Bilgisayar başında",
                "Yürüyüş yaparak",
                "Kitap okuyarak",
                "Arkadaşları ile buluşarak"
                ),
            GetQuestion(
                "Onu en çok ne sevindirir",
                user,
                "Kayıp parayı bulmak",
                "Tuttuğu takımın galibiyeti",
                "Süpriz hediye almak",
                "Alışveriş mağazasındaki indirimler",
                "Çekilişile telefon kazanmak"
                )
        };
    }

    private Question GetQuestion(string text, ApplicationUser user, params string[] answers)
    {
        return new Question()
        {
            Text = text,
            CreatorUserId = user.Id,
            CreateDate = DateTime.Now.ToIstanbulDateTime(),
            QuestionType = QuestionTypes.Text,
            AnswerType = AnswerTypes.Singular,
            IsPublic = true,
            Answers = answers.Select((x,index) => new QuestionAnswer()
            {
                Text = x,
                CreatorUserId = user.Id,
                DisplayOrder = (byte)index,
                CreateDate = DateTime.Now.ToIstanbulDateTime()
            }).ToList()
        };
    }
}