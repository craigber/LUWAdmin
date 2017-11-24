using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class Renewals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "WhenLastRenewalSent",
                table: "MemberChapters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenLastRenewalSent",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhenLastRenewalSent",
                table: "MemberChapters");

            migrationBuilder.DropColumn(
                name: "WhenLastRenewalSent",
                table: "AspNetUsers");
        }
    }
}
