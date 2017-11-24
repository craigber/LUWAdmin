using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class NotNullMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MeetingWeek",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MeetingDay",
                table: "Chapters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Chapters",
                maxLength: 2000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MeetingWeek",
                table: "Chapters",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "MeetingDay",
                table: "Chapters",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Chapters",
                maxLength: 2000,
                nullable: false);
        }
    }
}
