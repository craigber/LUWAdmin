using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class AddSenderInfoToTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReplyToEmailAddress",
                table: "EmailTemplates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendFromEmailAddress",
                table: "EmailTemplates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendFromName",
                table: "EmailTemplates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyToEmailAddress",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "SendFromEmailAddress",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "SendFromName",
                table: "EmailTemplates");
        }
    }
}
