using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CodingChallenge.Models;

namespace CodingChallenge.Data
{
    public class CodingChallengeContext : DbContext
    {
        public CodingChallengeContext (DbContextOptions<CodingChallengeContext> options)
            : base(options)
        {
        }

        public DbSet<CodingChallenge.Models.Contact> Contact { get; set; } = default!;

        public DbSet<CodingChallenge.Models.Address> Address { get; set; } = default!;

        public DbSet<CodingChallenge.Models.Combined> Combined { get; set; } = default!;
    }
}
