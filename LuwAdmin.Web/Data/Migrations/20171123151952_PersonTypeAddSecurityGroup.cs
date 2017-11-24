using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class PersonTypeAddSecurityGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMemberDefault",
                table: "PersonTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNonMemberDefault",
                table: "PersonTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityGroup",
                table: "PersonTypes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMemberDefault",
                table: "PersonTypes");

            migrationBuilder.DropColumn(
                name: "IsNonMemberDefault",
                table: "PersonTypes");

            migrationBuilder.DropColumn(
                name: "SecurityGroup",
                table: "PersonTypes");
        }
    }
}
