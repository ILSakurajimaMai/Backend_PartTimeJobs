using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTJ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileAndRenameCVTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Profiles_ProfileId",
                schema: "jobs",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileCertificates_Profiles_ProfileId",
                schema: "seeker",
                table: "ProfileCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileEducations_Profiles_ProfileId",
                schema: "seeker",
                table: "ProfileEducations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileExperiences_Profiles_ProfileId",
                schema: "seeker",
                table: "ProfileExperiences");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileSkills_Profiles_ProfileId",
                schema: "seeker",
                table: "ProfileSkills");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_UserId",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_UserId_IsDefault",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileSkills",
                schema: "seeker",
                table: "ProfileSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileExperiences",
                schema: "seeker",
                table: "ProfileExperiences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileEducations",
                schema: "seeker",
                table: "ProfileEducations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileCertificates",
                schema: "seeker",
                table: "ProfileCertificates");

            migrationBuilder.DropColumn(
                name: "Bio",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ExpectedGraduationDate",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "GPA",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "GitHubUrl",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Major",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ResumeUrl",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "StudentId",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "TargetPosition",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "University",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "YearOfStudy",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.RenameTable(
                name: "ProfileSkills",
                schema: "seeker",
                newName: "CVSkills",
                newSchema: "seeker");

            migrationBuilder.RenameTable(
                name: "ProfileExperiences",
                schema: "seeker",
                newName: "CVExperiences",
                newSchema: "seeker");

            migrationBuilder.RenameTable(
                name: "ProfileEducations",
                schema: "seeker",
                newName: "CVEducations",
                newSchema: "seeker");

            migrationBuilder.RenameTable(
                name: "ProfileCertificates",
                schema: "seeker",
                newName: "CVCertificates",
                newSchema: "seeker");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileSkills_ProfileId",
                schema: "seeker",
                table: "CVSkills",
                newName: "IX_CVSkills_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileExperiences_ProfileId",
                schema: "seeker",
                table: "CVExperiences",
                newName: "IX_CVExperiences_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileEducations_ProfileId",
                schema: "seeker",
                table: "CVEducations",
                newName: "IX_CVEducations_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileCertificates_ProfileId",
                schema: "seeker",
                table: "CVCertificates",
                newName: "IX_CVCertificates_ProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CVSkills",
                schema: "seeker",
                table: "CVSkills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CVExperiences",
                schema: "seeker",
                table: "CVExperiences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CVEducations",
                schema: "seeker",
                table: "CVEducations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CVCertificates",
                schema: "seeker",
                table: "CVCertificates",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CVs",
                schema: "seeker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TargetPosition = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    University = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Major = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GPA = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    YearOfStudy = table.Column<int>(type: "int", nullable: true),
                    ExpectedGraduationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResumeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GitHubUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVs_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "seeker",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CVs_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 778, DateTimeKind.Utc).AddTicks(8566));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 778, DateTimeKind.Utc).AddTicks(9464));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 778, DateTimeKind.Utc).AddTicks(9466));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 778, DateTimeKind.Utc).AddTicks(9467));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 778, DateTimeKind.Utc).AddTicks(9472));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 778, DateTimeKind.Utc).AddTicks(9473));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 778, DateTimeKind.Utc).AddTicks(9475));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 778, DateTimeKind.Utc).AddTicks(9476));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 778, DateTimeKind.Utc).AddTicks(9478));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 824, DateTimeKind.Utc).AddTicks(3882));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 824, DateTimeKind.Utc).AddTicks(3892));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 16, 48, 56, 824, DateTimeKind.Utc).AddTicks(3893));

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                schema: "seeker",
                table: "Profiles",
                column: "UserId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.Sql("DELETE FROM [jobs].[ApplicationHistories]");
            migrationBuilder.Sql("DELETE FROM [jobs].[Applications]");
            migrationBuilder.Sql("DELETE FROM [seeker].[CVCertificates]");
            migrationBuilder.Sql("DELETE FROM [seeker].[CVEducations]");
            migrationBuilder.Sql("DELETE FROM [seeker].[CVExperiences]");
            migrationBuilder.Sql("DELETE FROM [seeker].[CVSkills]");
            migrationBuilder.Sql(@"
                WITH RankedProfiles AS (
                    SELECT Id, UserId, ROW_NUMBER() OVER (PARTITION BY UserId ORDER BY Id ASC) AS rn
                    FROM [seeker].[Profiles]
                )
                DELETE FROM [seeker].[Profiles] WHERE Id IN (SELECT Id FROM RankedProfiles WHERE rn > 1)
            ");

            migrationBuilder.CreateIndex(
                name: "IX_CVs_ProfileId",
                schema: "seeker",
                table: "CVs",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CVs_UserId",
                schema: "seeker",
                table: "CVs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CVs_UserId_IsDefault",
                schema: "seeker",
                table: "CVs",
                columns: new[] { "UserId", "IsDefault" },
                unique: true,
                filter: "[IsDeleted] = 0 AND [IsDefault] = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_CVs_ProfileId",
                schema: "jobs",
                table: "Applications",
                column: "ProfileId",
                principalSchema: "seeker",
                principalTable: "CVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CVCertificates_CVs_ProfileId",
                schema: "seeker",
                table: "CVCertificates",
                column: "ProfileId",
                principalSchema: "seeker",
                principalTable: "CVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CVEducations_CVs_ProfileId",
                schema: "seeker",
                table: "CVEducations",
                column: "ProfileId",
                principalSchema: "seeker",
                principalTable: "CVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CVExperiences_CVs_ProfileId",
                schema: "seeker",
                table: "CVExperiences",
                column: "ProfileId",
                principalSchema: "seeker",
                principalTable: "CVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CVSkills_CVs_ProfileId",
                schema: "seeker",
                table: "CVSkills",
                column: "ProfileId",
                principalSchema: "seeker",
                principalTable: "CVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_CVs_ProfileId",
                schema: "jobs",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_CVCertificates_CVs_ProfileId",
                schema: "seeker",
                table: "CVCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_CVEducations_CVs_ProfileId",
                schema: "seeker",
                table: "CVEducations");

            migrationBuilder.DropForeignKey(
                name: "FK_CVExperiences_CVs_ProfileId",
                schema: "seeker",
                table: "CVExperiences");

            migrationBuilder.DropForeignKey(
                name: "FK_CVSkills_CVs_ProfileId",
                schema: "seeker",
                table: "CVSkills");

            migrationBuilder.DropTable(
                name: "CVs",
                schema: "seeker");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_UserId",
                schema: "seeker",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CVSkills",
                schema: "seeker",
                table: "CVSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CVExperiences",
                schema: "seeker",
                table: "CVExperiences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CVEducations",
                schema: "seeker",
                table: "CVEducations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CVCertificates",
                schema: "seeker",
                table: "CVCertificates");

            migrationBuilder.RenameTable(
                name: "CVSkills",
                schema: "seeker",
                newName: "ProfileSkills",
                newSchema: "seeker");

            migrationBuilder.RenameTable(
                name: "CVExperiences",
                schema: "seeker",
                newName: "ProfileExperiences",
                newSchema: "seeker");

            migrationBuilder.RenameTable(
                name: "CVEducations",
                schema: "seeker",
                newName: "ProfileEducations",
                newSchema: "seeker");

            migrationBuilder.RenameTable(
                name: "CVCertificates",
                schema: "seeker",
                newName: "ProfileCertificates",
                newSchema: "seeker");

            migrationBuilder.RenameIndex(
                name: "IX_CVSkills_ProfileId",
                schema: "seeker",
                table: "ProfileSkills",
                newName: "IX_ProfileSkills_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_CVExperiences_ProfileId",
                schema: "seeker",
                table: "ProfileExperiences",
                newName: "IX_ProfileExperiences_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_CVEducations_ProfileId",
                schema: "seeker",
                table: "ProfileEducations",
                newName: "IX_ProfileEducations_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_CVCertificates_ProfileId",
                schema: "seeker",
                table: "ProfileCertificates",
                newName: "IX_ProfileCertificates_ProfileId");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                schema: "seeker",
                table: "Profiles",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedGraduationDate",
                schema: "seeker",
                table: "Profiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GPA",
                schema: "seeker",
                table: "Profiles",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                schema: "seeker",
                table: "Profiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubUrl",
                schema: "seeker",
                table: "Profiles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                schema: "seeker",
                table: "Profiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                schema: "seeker",
                table: "Profiles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Major",
                schema: "seeker",
                table: "Profiles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumeUrl",
                schema: "seeker",
                table: "Profiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                schema: "seeker",
                table: "Profiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetPosition",
                schema: "seeker",
                table: "Profiles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "seeker",
                table: "Profiles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "University",
                schema: "seeker",
                table: "Profiles",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearOfStudy",
                schema: "seeker",
                table: "Profiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileSkills",
                schema: "seeker",
                table: "ProfileSkills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileExperiences",
                schema: "seeker",
                table: "ProfileExperiences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileEducations",
                schema: "seeker",
                table: "ProfileEducations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileCertificates",
                schema: "seeker",
                table: "ProfileCertificates",
                column: "Id");

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 183, DateTimeKind.Utc).AddTicks(9501));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 184, DateTimeKind.Utc).AddTicks(405));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 184, DateTimeKind.Utc).AddTicks(406));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 184, DateTimeKind.Utc).AddTicks(408));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 184, DateTimeKind.Utc).AddTicks(409));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 184, DateTimeKind.Utc).AddTicks(410));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 184, DateTimeKind.Utc).AddTicks(412));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 184, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 184, DateTimeKind.Utc).AddTicks(415));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 217, DateTimeKind.Utc).AddTicks(7755));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 217, DateTimeKind.Utc).AddTicks(7762));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 47, 12, 217, DateTimeKind.Utc).AddTicks(7763));

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                schema: "seeker",
                table: "Profiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId_IsDefault",
                schema: "seeker",
                table: "Profiles",
                columns: new[] { "UserId", "IsDefault" },
                unique: true,
                filter: "[IsDeleted] = 0 AND [IsDefault] = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Profiles_ProfileId",
                schema: "jobs",
                table: "Applications",
                column: "ProfileId",
                principalSchema: "seeker",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileCertificates_Profiles_ProfileId",
                schema: "seeker",
                table: "ProfileCertificates",
                column: "ProfileId",
                principalSchema: "seeker",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileEducations_Profiles_ProfileId",
                schema: "seeker",
                table: "ProfileEducations",
                column: "ProfileId",
                principalSchema: "seeker",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileExperiences_Profiles_ProfileId",
                schema: "seeker",
                table: "ProfileExperiences",
                column: "ProfileId",
                principalSchema: "seeker",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileSkills_Profiles_ProfileId",
                schema: "seeker",
                table: "ProfileSkills",
                column: "ProfileId",
                principalSchema: "seeker",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
