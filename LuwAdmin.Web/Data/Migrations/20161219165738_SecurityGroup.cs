using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class SecurityGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailUseSsl",
                table: "Tenants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityGroup",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailUseSsl",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SecurityGroup",
                table: "AspNetUsers");
        }
    }
}
