using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ORC.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateFrom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Registrations");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Registrations",
                newName: "TechnicalSkills");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Registrations",
                newName: "TeamMembers");

            migrationBuilder.RenameColumn(
                name: "Goals",
                table: "Registrations",
                newName: "RoboticsExperience");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Registrations",
                newName: "RelevantProjects");

            migrationBuilder.RenameColumn(
                name: "Experience",
                table: "Registrations",
                newName: "LeaderPhone");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Registrations",
                newName: "LeaderName");

            migrationBuilder.AddColumn<bool>(
                name: "AgreeToMedia",
                table: "Registrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AgreeToRules",
                table: "Registrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AnticipatedChallenges",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasPriorExperience",
                table: "Registrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LeaderEmail",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PriorExperienceDetails",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamSize",
                table: "Registrations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "VerifyInformation",
                table: "Registrations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgreeToMedia",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "AgreeToRules",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "AnticipatedChallenges",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "HasPriorExperience",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "LeaderEmail",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "PriorExperienceDetails",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "TeamSize",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "VerifyInformation",
                table: "Registrations");

            migrationBuilder.RenameColumn(
                name: "TechnicalSkills",
                table: "Registrations",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "TeamMembers",
                table: "Registrations",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "RoboticsExperience",
                table: "Registrations",
                newName: "Goals");

            migrationBuilder.RenameColumn(
                name: "RelevantProjects",
                table: "Registrations",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "LeaderPhone",
                table: "Registrations",
                newName: "Experience");

            migrationBuilder.RenameColumn(
                name: "LeaderName",
                table: "Registrations",
                newName: "Email");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Registrations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
