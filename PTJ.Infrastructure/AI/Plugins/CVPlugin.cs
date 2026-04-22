using System.ComponentModel;
using System.Text.Json;
using Microsoft.SemanticKernel;
using PTJ.Application.Services;

namespace PTJ.Infrastructure.AI.Plugins;

public class CVPlugin
{
    private readonly ICVService _cvService;

    public CVPlugin(ICVService cvService)
    {
        _cvService = cvService;
    }

    [KernelFunction("get_my_profile")]
    [Description("Retrieves the professional profile (CV) of the current user.")]
    [return: Description("A JSON string containing skills, education, experience, and other CV details.")]
    public async Task<string> GetMyCVAsync(
        [Description("The ID of the current user.")] int userId
    )
    {
        var result = await _cvService.GetDefaultByUserIdAsync(userId);

        if (!result.Success || result.Data == null)
        {
            return "CV not found. The user has not created a CV yet.";
        }

        var cv = result.Data;

        var simpleCV = new
        {
            cv.FullName,
            cv.Email,
            cv.PhoneNumber,
            cv.Major,
            cv.GPA,
            cv.University,
            Skills = cv.Skills.Select(s => s.SkillName),
            Experience = cv.Experiences.Select(e => new
            {
                e.CompanyName,
                e.Position,
                Duration = $"{(e.EndDate?.ToString("MM/yyyy") ?? "Present")} - {e.StartDate:MM/yyyy}"
            }),
            Education = cv.Educations.Select(e => new
            {
                e.InstitutionName,
                e.Degree,
                e.FieldOfStudy
            })
        };

        return JsonSerializer.Serialize(simpleCV);
    }
}
