using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class chapters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Chapter",
                table: "Chapter");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapters",
                table: "Chapter",
                column: "Id");

            migrationBuilder.RenameTable(
                name: "Chapter",
                newName: "Chapters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapter",
                table: "Chapters",
                column: "Id");

            migrationBuilder.RenameTable(
                name: "Chapters",
                newName: "Chapter");
        }
    }
}
