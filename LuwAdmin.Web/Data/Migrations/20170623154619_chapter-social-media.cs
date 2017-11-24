using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class chaptersocialmedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "Chapters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "Chapters");
        }
    }
}
