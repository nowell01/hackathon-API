using HackathonApi.Metadata;
using HackathonApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HackathonApi.DTO
{
    [ModelMetadataType(typeof(MemberMetadata))]
    public class RegionDTO
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }

        public List<MemberDTO>? Members { get; set; }
    }
}