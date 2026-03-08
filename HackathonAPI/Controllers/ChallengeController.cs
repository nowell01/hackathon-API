using HackathonApi.Data;
using HackathonApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChallengeController : ControllerBase
{
    private readonly HackathonContext _context;

    public ChallengeController(HackathonContext context)
    {
        _context = context;
    }

    // GET: api/Challenge
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChallengeDTO>>> GetChallenges()
    {
        var challengeDTOs = await _context.Challenges
                 .Select(c => new ChallengeDTO
                 {
                     ID = c.ID,
                     Code = c.Code,
                     Name = c.Name
                 })
                 .ToListAsync();

        if (challengeDTOs.Count() > 0)
        {
            return challengeDTOs;
        }
        else
        {
            return NotFound(new { message = "Error: No Challenge records found in the database." });
        }
    }

    // GET: api/Challenge/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChallengeDTO>> GetChallenge(int id)
    {
        var challengeDTOs = await _context.Challenges
                 .Select(c => new ChallengeDTO
                 {
                     ID = c.ID,
                     Code = c.Code,
                     Name = c.Name
                 })
                 .FirstOrDefaultAsync(c => c.ID == id);

        if (challengeDTOs == null)
        {
            return NotFound(new { message = "Error: No Challenge records found in the database." });
        }

        return challengeDTOs;
    }

    // GET: api/Challenge/inc
    [HttpGet("inc")]
    public async Task<ActionResult<IEnumerable<ChallengeDTO>>> GetChallengesInc()
    {
        var challengeDTOs = await _context.Challenges
                 .Select(c => new ChallengeDTO
                 {
                     ID = c.ID,
                     Code = c.Code,
                     Name = c.Name,
                     Members = c.Members.Select(m => new MemberDTO
                     {
                         ID = m.ID,
                         FirstName = m.FirstName,
                         MiddleName = m.MiddleName,
                         LastName = m.LastName,
                         MemberCode = m.MemberCode,
                         DOB = m.DOB,
                         SkillRating = m.SkillRating,
                         YearsExperience = m.YearsExperience,
                         Category = m.Category,
                         Organization = m.Organization,
                         RegionID = m.RegionID,
                         ChallengeID = m.ChallengeID
                     }).ToList()
                 })
                 .ToListAsync();

        if (challengeDTOs.Count() > 0)
        {
            return challengeDTOs;
        }
        else
        {
            return NotFound(new { message = "Error: No Challenge records found in the database." });
        }
    }

    // GET: api/Challenge/inc/{id}
    [HttpGet("inc/{id:int}")]
    public async Task<ActionResult<ChallengeDTO>> GetChallengeInc(int id)
    {
        var challengeDTOs = await _context.Challenges
                 .Select(c => new ChallengeDTO
                 {
                     ID = c.ID,
                     Code = c.Code,
                     Name = c.Name,
                     Members = c.Members.Select(m => new MemberDTO
                     {
                         ID = m.ID,
                         FirstName = m.FirstName,
                         MiddleName = m.MiddleName,
                         LastName = m.LastName,
                         MemberCode = m.MemberCode,
                         DOB = m.DOB,
                         SkillRating = m.SkillRating,
                         YearsExperience = m.YearsExperience,
                         Category = m.Category,
                         Organization = m.Organization,
                         RegionID = m.RegionID,
                         ChallengeID = m.ChallengeID
                     }).ToList()
                 })
                 .FirstOrDefaultAsync(r => r.ID == id);

        if (challengeDTOs == null)
        {
            return NotFound(new { message = "Error: No Challenge records found in the database." });
        }

        return challengeDTOs;
    }
}