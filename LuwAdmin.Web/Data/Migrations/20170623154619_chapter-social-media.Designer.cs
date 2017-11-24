using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using LuwAdmin.Web.Data;

namespace LuwAdmin.Web.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170623154619_chapter-social-media")]
    partial class chaptersocialmedia
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LuwAdmin.Web.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityGroup");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("State");

                    b.Property<string>("Status")
                        .HasMaxLength(20);

                    b.Property<string>("Street1");

                    b.Property<string>("Street2");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<DateTime>("WhenExpires");

                    b.Property<DateTime>("WhenJoined");

                    b.Property<string>("ZipCode");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("LuwAdmin.Web.Models.ApplicationUserNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddedBy");

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Note");

                    b.Property<DateTime>("WhenAdded");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUserNotes");
                });

            modelBuilder.Entity("LuwAdmin.Web.Models.Chapter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .HasMaxLength(100);

                    b.Property<string>("Description")
                        .HasMaxLength(2000);

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<string>("EndTime");

                    b.Property<string>("Facebook");

                    b.Property<int?>("MeetingDay");

                    b.Property<int?>("MeetingWeek");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Notes");

                    b.Property<string>("Phone")
                        .HasMaxLength(15);

                    b.Property<string>("StartTime");

                    b.Property<string>("State")
                        .HasMaxLength(2);

                    b.Property<string>("Street1")
                        .HasMaxLength(100);

                    b.Property<string>("Street2")
                        .HasMaxLength(100);

                    b.Property<string>("SubName")
                        .HasMaxLength(100);

                    b.Property<string>("Twitter");

                    b.Property<string>("Url")
                        .HasMaxLength(100);

                    b.Property<string>("Venue")
                        .HasMaxLength(100);

                    b.Property<string>("Zip")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("LuwAdmin.Web.Models.ChapterMeeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChapterId");

                    b.Property<string>("City")
                        .HasMaxLength(100);

                    b.Property<string>("Description");

                    b.Property<string>("EndTime");

                    b.Property<int?>("MeetingDay");

                    b.Property<string>("MeetingType");

                    b.Property<int?>("MeetingWeek");

                    b.Property<string>("StartTime");

                    b.Property<string>("State")
                        .HasMaxLength(2);

                    b.Property<string>("Street1")
                        .HasMaxLength(100);

                    b.Property<string>("Street2")
                        .HasMaxLength(100);

                    b.Property<string>("Venue")
                        .HasMaxLength(100);

                    b.Property<string>("Zip")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("ChapterMeetings");
                });

            modelBuilder.Entity("LuwAdmin.Web.Models.EmailAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("EmailAssignments");
                });

            modelBuilder.Entity("LuwAdmin.Web.Models.EmailTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<int>("EmailAssignmentId");

                    b.Property<string>("Name");

                    b.Property<string>("ReplyToEmailAddress");

                    b.Property<string>("SendFromEmailAddress");

                    b.Property<string>("SendFromName");

                    b.Property<string>("Subject");

                    b.HasKey("Id");

                    b.ToTable("EmailTemplates");
                });

            modelBuilder.Entity("LuwAdmin.Web.Models.MemberChapter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<int>("ChapterId");

                    b.Property<DateTime>("WhenJoined");

                    b.Property<DateTime?>("WhenLeft");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("ChapterId");

                    b.ToTable("MemberChapters");
                });

            modelBuilder.Entity("LuwAdmin.Web.Models.Tenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .HasMaxLength(100);

                    b.Property<bool>("DoesEmailRequireLogon");

                    b.Property<string>("EmailLogon")
                        .HasMaxLength(100);

                    b.Property<string>("EmailPassword");

                    b.Property<int>("EmailPort");

                    b.Property<string>("EmailProtocol");

                    b.Property<string>("EmailServer")
                        .HasMaxLength(50);

                    b.Property<bool>("EmailUseSsl");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("State")
                        .HasMaxLength(2);

                    b.Property<string>("Street1")
                        .HasMaxLength(100);

                    b.Property<string>("Street2")
                        .HasMaxLength(100);

                    b.Property<string>("ZipCode");

                    b.HasKey("Id");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("LuwAdmin.Web.Models.MemberChapter", b =>
                {
                    b.HasOne("LuwAdmin.Web.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Chapters")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("LuwAdmin.Web.Models.Chapter", "Chapter")
                        .WithMany()
                        .HasForeignKey("ChapterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("LuwAdmin.Web.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LuwAdmin.Web.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LuwAdmin.Web.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
