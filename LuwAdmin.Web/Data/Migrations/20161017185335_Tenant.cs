using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class Tenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(nullable: true),
                    EmailServer = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Street1 = table.Column<string>(nullable: true),
                    Street2 = table.Column<string>(nullable: true),
                    Zip = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "MemberChapters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberChapters_ApplicationUserId",
                table: "MemberChapters",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberChapters_ChapterId",
                table: "MemberChapters",
                column: "ChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberChapters_Chapters_ChapterId",
                table: "MemberChapters",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberChapters_AspNetUsers_ApplicationUserId",
                table: "MemberChapters");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberChapters_Chapters_ChapterId",
                table: "MemberChapters");

            migrationBuilder.DropIndex(
                name: "IX_MemberChapters_ApplicationUserId",
                table: "MemberChapters");

            migrationBuilder.DropIndex(
                name: "IX_MemberChapters_ChapterId",
                table: "MemberChapters");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "MemberChapters",
                nullable: true);
        }
    }
}
