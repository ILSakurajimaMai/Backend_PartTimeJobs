using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTJ.Infrastructure.Migrations
{
    [Migration("20260420174000_AddMultiProfileSupport")]
    public partial class AddMultiProfileSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_UserId",
                schema: "seeker",
                table: "CVs");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                schema: "seeker",
                table: "CVs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TargetPosition",
                schema: "seeker",
                table: "CVs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "seeker",
                table: "CVs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.Sql(
                """
                ;WITH rankedProfiles AS (
                    SELECT
                        Id,
                        ROW_NUMBER() OVER (PARTITION BY UserId ORDER BY Id) AS rn
                    FROM [seeker].[Profiles]
                    WHERE [IsDeleted] = 0
                )
                UPDATE p
                SET
                    [IsDefault] = CASE WHEN rp.rn = 1 THEN 1 ELSE 0 END,
                    [Title] = COALESCE([Title], CONCAT(N'CV ', rp.rn))
                FROM [seeker].[Profiles] p
                INNER JOIN rankedProfiles rp ON rp.Id = p.Id;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                schema: "seeker",
                table: "CVs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId_IsDefault",
                schema: "seeker",
                table: "CVs",
                columns: new[] { "UserId", "IsDefault" },
                unique: true,
                filter: "[IsDeleted] = 0 AND [IsDefault] = 1");

            migrationBuilder.Sql(
                """
                DECLARE @schemaName sysname;
                SELECT TOP (1) @schemaName = OBJECT_SCHEMA_NAME(object_id)
                FROM sys.triggers
                WHERE name = N'trg_SyncUserDataToProfile';

                IF @schemaName IS NOT NULL
                BEGIN
                    EXEC(N'DROP TRIGGER ' + QUOTENAME(@schemaName) + N'.[trg_SyncUserDataToProfile]');
                END
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_UserId_IsDefault",
                schema: "seeker",
                table: "CVs");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_UserId",
                schema: "seeker",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                schema: "seeker",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "TargetPosition",
                schema: "seeker",
                table: "CVs");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "seeker",
                table: "CVs");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                schema: "seeker",
                table: "CVs",
                column: "UserId",
                unique: true,
                filter: "[IsDeleted] = 0");
        }
    }
}
