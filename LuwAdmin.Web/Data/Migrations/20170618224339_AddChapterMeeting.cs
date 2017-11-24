using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class AddChapterMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChapterMeetings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChapterId = table.Column<int>(nullable: false),
                    City = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EndTime = table.Column<string>(nullable: true),
                    MeetingDay = table.Column<int>(nullable: true),
                    MeetingType = table.Column<string>(nullable: true),
                    MeetingWeek = table.Column<int>(nullable: true),
                    StartTime = table.Column<string>(nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    Street1 = table.Column<string>(maxLength: 100, nullable: true),
                    Street2 = table.Column<string>(maxLength: 100, nullable: true),
                    Venue = table.Column<string>(maxLength: 100, nullable: true),
                    Zip = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterMeetings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterMeetings");
        }
    }
}
