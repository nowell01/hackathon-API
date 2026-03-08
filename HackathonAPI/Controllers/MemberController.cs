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
        var memberDTOs = await _context.Members
                .Include(e => e.Region)
                .Where(e => e.RegionID == id)
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
                    RowVersion = m.RowVersion,
                    RegionID = m.RegionID,
                    Region = m.Region != null ? new RegionDTO
                    {
                        ID = m.Region.ID,
                        Code = m.Region.Code,
                        Name = m.Region.Name,

                    } : null
                })
                .ToListAsync();

        if (memberDTOs.Count() > 0)
        {
            return memberDTOs;
        }
        else
        {
            return NotFound(new { message = "Error: No Members for the specified Region." });
        }
    }

    // GET: api/Member/ByChallenge/{id}
    [HttpGet("ByChallenge/{id:int}")]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMemberByChallenge(int id)
    {
        var memberDTOs = await _context.Members
                .Include(e => e.Challenge)
                .Where(e => e.ChallengeID == id)
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
                    RowVersion = m.RowVersion,
                    ChallengeID = m.ChallengeID,
                    Challenge = m.Challenge != null ? new ChallengeDTO
                    {
                        ID = m.Challenge.ID,
                        Code = m.Challenge.Code,
                        Name = m.Challenge.Name,

                    } : null
                })
                .ToListAsync();

        if (memberDTOs.Count() > 0)
        {
            return memberDTOs;
        }
        else
        {
            return NotFound(new { message = "Error: No Members for the specified Region." });
        }
    }

    // POST: api/Member
    [HttpPost]
    public async Task<ActionResult<MemberDTO>> PostMember(MemberDTO memberdto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Member member = new Member
        {
            FirstName = memberdto.FirstName,
            MiddleName = memberdto.MiddleName,
            LastName = memberdto.LastName,
            MemberCode = memberdto.MemberCode,
            DOB = memberdto.DOB,
            SkillRating = memberdto.SkillRating,
            YearsExperience = memberdto.YearsExperience,
            Category = memberdto.Category,
            Organization = memberdto.Organization,
            RegionID = memberdto.RegionID,
            ChallengeID = memberdto.ChallengeID
        };

        try
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            memberdto.ID = member.ID;
            memberdto.RowVersion = member.RowVersion;

            return CreatedAtAction(nameof(GetMember), new { id = member.ID }, memberdto);
        }
        catch (DbUpdateException ex)
        {
            if (ex.GetBaseException().Message.Contains("UNIQUE"))
            {
                return BadRequest(new { message = "Unable to save: Duplicate MemberCode number." });
            }
            else
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }
        }
    }

    // PUT: api/Member/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutMember(int id, MemberDTO memberdto)
    {
        if (id != memberdto.ID)
        {
            return BadRequest(new { message = "Error: ID does not match Member" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var memberToUpdate = await _context.Members.FindAsync(id);

        if (memberToUpdate == null)
        {
            return NotFound(new { message = "Error: Member record not found." });
        }

        if (memberdto.RowVersion != null)
        {
            if (!memberToUpdate.RowVersion.SequenceEqual(memberdto.RowVersion))
            {
                return Conflict(new { message = "Concurrency Error: Member has been changed by another user.  Try editing the record again." });
            }
        }

        memberToUpdate.ID = memberdto.ID;
        memberToUpdate.FirstName = memberdto.FirstName;
        memberToUpdate.MiddleName = memberdto.MiddleName;
        memberToUpdate.LastName = memberdto.LastName;
        memberToUpdate.MemberCode = memberdto.MemberCode;
        memberToUpdate.DOB = memberdto.DOB;
        memberToUpdate.SkillRating = memberdto.SkillRating;
        memberToUpdate.RowVersion = memberdto.RowVersion;
        memberToUpdate.YearsExperience = memberdto.YearsExperience;
        memberToUpdate.Category = memberdto.Category;
        memberToUpdate.Organization = memberdto.Organization;
        memberToUpdate.RegionID = memberdto.RegionID;
        memberToUpdate.ChallengeID = memberdto.ChallengeID;

        //Put the original RowVersion value in the OriginalValues collection for the entity
        _context.Entry(memberToUpdate).Property("RowVersion").OriginalValue = memberdto.RowVersion;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MemberExists(id))
            {
                return Conflict(new { message = "Concurrency Error: Member has been Removed." });
            }
            else
            {
                return Conflict(new { message = "Concurrency Error: Member has been updated by another user.  Back out and try editing the record again." });
            }
        }
        catch (DbUpdateException dex)
        {
            if (dex.GetBaseException().Message.Contains("UNIQUE"))
            {
                return BadRequest(new { message = "Unable to save: Duplicate MemberCode number." });
            }
            else
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }
        }
    }

    // DELETE: api/Member/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member == null) return NotFound(new { message = "Delete Error: Member has already been removed." });

        try
        {
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(new { message = "Delete Error: Unable to delete Member." });
        }
    }
    private bool MemberExists(int id)
    {
        return _context.Members.Any(e => e.ID == id);
    }
}