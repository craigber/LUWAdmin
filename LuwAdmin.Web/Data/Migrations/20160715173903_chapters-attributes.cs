using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class chaptersattributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Zip",
                table: "Chapters",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "Chapters",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Chapters",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubName",
                table: "Chapters",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street2",
                table: "Chapters",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street1",
                table: "Chapters",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Chapters",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Chapters",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Chapters",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Chapters",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Chapters",
                maxLength: 2000,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Chapters",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Zip",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubName",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street2",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street1",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Chapters",
                nullable: true);
        }
    }
}
