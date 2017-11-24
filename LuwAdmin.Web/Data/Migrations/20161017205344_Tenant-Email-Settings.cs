using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LuwAdmin.Web.Data.Migrations
{
    public partial class TenantEmailSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Tenants");

            migrationBuilder.AddColumn<bool>(
                name: "DoesEmailRequireLogon",
                table: "Tenants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmailLogon",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailPassword",
                table: "Tenants",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmailPort",
                table: "Tenants",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Tenants",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street2",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street1",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Tenants",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tenants",
                maxLength: 200,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "EmailServer",
                table: "Tenants",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Tenants",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoesEmailRequireLogon",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EmailLogon",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EmailPassword",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EmailPort",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Tenants");

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Tenants",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street2",
                table: "Tenants",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street1",
                table: "Tenants",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Tenants",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tenants",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailServer",
                table: "Tenants",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Tenants",
                nullable: true);
        }
    }
}
