using HackathonApi.Data;
using HackathonApi.DTO;
using HackathonApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController : ControllerBase
{
    private readonly HackathonContext _context;

    public MemberController(HackathonContext context)
    {
        _context = context;
    }

    // GET: api/Member
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembers()
    {

        var memberDTOs = await _context.Members
                 .Select(m => new MemberDTO
                 {
                     ID = m.ID,
                     FirstName = m.FirstName,
                     MiddleName = m.MiddleName,
                     LastName = m.LastName,
                     RegionID = m.RegionID,
                     ChallengeID = m.ChallengeID,
                     RowVersion = m.RowVersion
                 })
                 .ToListAsync();

        if (memberDTOs.Count() > 0)
        {
            return memberDTOs;
        }
        else
        {
            return NotFound(new { message = "Error: No Members records found in the database." });
        }
    }

    // GET: api/Member/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDTO>> GetMember(int id)
    {
        var memberDTO = await _context.Members
                .Select(m => new MemberDTO
                {
                    ID = m.ID,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    RegionID = m.RegionID,
                    ChallengeID = m.ChallengeID,
                    RowVersion = m.RowVersion
                })
                .FirstOrDefaultAsync(m => m.ID == id);

        if (memberDTO == null)
        {
            return NotFound(new { message = "Error: That Member was not found in the database." });
        }

        return memberDTO;
    }

    // GET: api/Member/ByRegion/{id}
    [HttpGet("ByRegion/{id:int}")]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMemberByRegion(int id)
    {
        var members = await _context.Members
            .AsNoTracking()
            .Include(m => m.Region)
            .Include(m => m.Challenge)
            .Where(m => m.RegionID == id)
            .ToListAsync();

        return Ok(members);
    }

    // GET: api/Member/ByChallenge/{id}
    [HttpGet("ByChallenge/{id:int}")]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMemberByChallenge(int id)
    {
        var members = await _context.Members
            .AsNoTracking()
            .Include(m => m.Region)
            .Include(m => m.Challenge)
            .Where(m => m.ChallengeID == id)
            .ToListAsync();

        return Ok(members);
    }

    // POST: api/Member
    [HttpPost]
    public async Task<ActionResult<MemberDTO>> PostMember(MemberDTO dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var member = new Member
        {
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            MemberCode = dto.MemberCode,
            DOB = dto.DOB,
            SkillRating = dto.SkillRating,
            YearsExperience = dto.YearsExperience,
            Category = dto.Category,
            Organization = dto.Organization,
            RegionID = dto.RegionID,
            ChallengeID = dto.ChallengeID
        };

        _context.Members.Add(member);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(BuildDbErrorMessage(ex));
        }

        var created = await _context.Members
            .AsNoTracking()
            .Select(m => new MemberDTO
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
                ChallengeID = m.ChallengeID,
                RowVersion = m.RowVersion
            })
            .FirstAsync(m => m.ID == member.ID);

        return CreatedAtAction(nameof(GetMember), new { id = created.ID }, created);
    }

    // PUT: api/Member/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutMember(int id, MemberDTO dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var member = await _context.Members.FirstOrDefaultAsync(m => m.ID == id);
        if (member == null) return NotFound();

        if (dto.RowVersion == null)
            return BadRequest("RowVersion is required for concurrency control.");

        member.FirstName = dto.FirstName;
        member.MiddleName = dto.MiddleName;
        member.LastName = dto.LastName;
        member.MemberCode = dto.MemberCode;
        member.DOB = dto.DOB;
        member.SkillRating = dto.SkillRating;
        member.YearsExperience = dto.YearsExperience;
        member.Category = dto.Category;
        member.Organization = dto.Organization;
        member.RegionID = dto.RegionID;
        member.ChallengeID = dto.ChallengeID;

        _context.Entry(member).Property(m => m.RowVersion).OriginalValue = dto.RowVersion;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("Concurrency conflict: the record was modified by another user.");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(BuildDbErrorMessage(ex));
        }

        return NoContent();
    }

    // DELETE: api/Member/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var member = await _context.Members.FirstOrDefaultAsync(m => m.ID == id);
        if (member == null) return NotFound();

        _context.Members.Remove(member);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(BuildDbErrorMessage(ex));
        }

        return NoContent();
    }

    private static string BuildDbErrorMessage(DbUpdateException ex)
    {
        return "Database error: " + (ex.InnerException?.Message ?? ex.Message);
    }
}