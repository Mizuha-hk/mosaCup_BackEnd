using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mosaCupBackendServer.Models.DbModels;

namespace mosaCupBackendServer.Data
{
    public class mosaCupBackendServerContext : DbContext
    {
        public mosaCupBackendServerContext (DbContextOptions<mosaCupBackendServerContext> options)
            : base(options)
        {
        }

        public DbSet<mosaCupBackendServer.Models.DbModels.UserData> UserData { get; set; } = default!;

        public DbSet<mosaCupBackendServer.Models.DbModels.Follow> Follow { get; set; } = default!;

        public DbSet<mosaCupBackendServer.Models.DbModels.Post> Post { get; set; } = default!;

        public DbSet<mosaCupBackendServer.Models.DbModels.Like> Like { get; set; } = default!;
    }
}
