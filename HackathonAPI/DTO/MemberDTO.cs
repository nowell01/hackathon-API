using HackathonApi.Metadata;
using HackathonApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HackathonApi.DTO
{
    [ModelMetadataType(typeof(MemberMetadata))]
    public class MemberDTO
    {
        public int ID { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? MemberCode { get; set; }

        public DateTime DOB { get; set; }
        public int SkillRating { get; set; }
        public int YearsExperience { get; set; }
        public string? Category { get; set; }
        public string? Organization { get; set; }
        public ChallengeDTO? Challenge { get; set; }
        public RegionDTO? Region { get; set; }
        public int RegionID { get; set; }
        public int ChallengeID { get; set; }
        public Byte[]? RowVersion { get; set; }
    }
}