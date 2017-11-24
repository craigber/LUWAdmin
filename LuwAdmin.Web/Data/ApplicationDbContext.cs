using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LuwAdmin.Web.Models;

namespace LuwAdmin.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

        }

        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ApplicationUserNote> ApplicationUserNotes { get; set; }
        public DbSet<MemberChapter> MemberChapters { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailAssignment> EmailAssignments { get; set; }

        public DbSet<ChapterMeeting> ChapterMeetings { get; set; }

        public DbSet<PersonType> PersonTypes { get; set; }
    }
}
