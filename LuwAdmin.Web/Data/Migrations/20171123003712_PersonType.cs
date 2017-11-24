using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class PersonType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChapterSplit = table.Column<int>(type: "int", nullable: false),
                    IsMember = table.Column<bool>(type: "bit", nullable: false),
                    LeagueSplit = table.Column<int>(type: "int", nullable: false),
                    MembershipFee = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsletterGraceDays = table.Column<int>(type: "int", nullable: false),
                    SendNewsletter = table.Column<bool>(type: "bit", nullable: false),
                    StartSendingRenewalDays = table.Column<int>(type: "int", nullable: false),
                    StopSendingRenewalDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonTypes");
        }
    }
}
