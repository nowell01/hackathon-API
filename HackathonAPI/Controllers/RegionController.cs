using HackathonApi.Data;
using HackathonApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionController : ControllerBase
{
    private readonly HackathonContext _context;

    public RegionController(HackathonContext context)
    {
        _context = context;
    }

    // GET: api/Region
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegionDTO>>> GetRegions()
    {
        var regionDTOs = await _context.Regions
                 .Select(r => new RegionDTO
                 {
                     ID = r.ID,
                     Code = r.Code,
                     Name = r.Name
                 })
                 .ToListAsync();

        if (regionDTOs.Count() > 0)
        {
            return regionDTOs;
        }
        else
        {
            return NotFound(new { message = "Error: No Region records found in the database." });
        }
    }

    // GET: api/Region/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RegionDTO>> GetRegion(int id)
    {
        var regionDTOs = await _context.Regions
                 .Select(r => new RegionDTO
                 {
                     ID = r.ID,
                     Code = r.Code,
                     Name = r.Name
                 })
                 .FirstOrDefaultAsync(r => r.ID == id);

        if (regionDTOs == null)
        {
            return NotFound(new { message = "Error: No Region records found in the database." });
        }

        return regionDTOs;
    }

    // GET: api/Region/inc
    [HttpGet("inc")]
    public async Task<ActionResult<IEnumerable<RegionDTO>>> GetRegionsInc()
    {
        var regionDTOs = await _context.Regions
                 .Select(r => new RegionDTO
                 {
                     ID = r.ID,
                     Code = r.Code,
                     Name = r.Name,
                     Members = r.Members.Select(m => new MemberDTO
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

        if (regionDTOs.Count() > 0)
        {
            return regionDTOs;
        }
        else
        {
            return NotFound(new { message = "Error: No Region records found in the database." });
        }
    }

    // GET: api/Region/inc/{id}
    [HttpGet("inc/{id:int}")]
    public async Task<ActionResult<RegionDTO>> GetRegionInc(int id)
    {
        var regionDTOs = await _context.Regions
                 .Select(r => new RegionDTO
                 {
                     ID = r.ID,
                     Code = r.Code,
                     Name = r.Name,
                     Members = r.Members.Select(m => new MemberDTO
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

        if (regionDTOs == null)
        {
            return NotFound(new { message = "Error: No Region records found in the database." });
        }

        return regionDTOs;
    }
}