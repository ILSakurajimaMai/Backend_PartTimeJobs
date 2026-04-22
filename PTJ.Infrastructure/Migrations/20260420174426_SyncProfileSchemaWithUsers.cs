using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTJ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncProfileSchemaWithUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 479, DateTimeKind.Utc).AddTicks(3039));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 479, DateTimeKind.Utc).AddTicks(3864));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 479, DateTimeKind.Utc).AddTicks(3866));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 479, DateTimeKind.Utc).AddTicks(3867));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 479, DateTimeKind.Utc).AddTicks(3869));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 479, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 479, DateTimeKind.Utc).AddTicks(3872));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 479, DateTimeKind.Utc).AddTicks(3873));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 479, DateTimeKind.Utc).AddTicks(3875));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 506, DateTimeKind.Utc).AddTicks(8660));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 506, DateTimeKind.Utc).AddTicks(8667));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 17, 44, 24, 506, DateTimeKind.Utc).AddTicks(8668));

            migrationBuilder.CreateIndex(
                name: "IX_JobPosts_CreatedByUserId",
                schema: "jobs",
                table: "JobPosts",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosts_Users_CreatedByUserId",
                schema: "jobs",
                table: "JobPosts",
                column: "CreatedByUserId",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPosts_Users_CreatedByUserId",
                schema: "jobs",
                table: "JobPosts");

            migrationBuilder.DropIndex(
                name: "IX_JobPosts_CreatedByUserId",
                schema: "jobs",
                table: "JobPosts");

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 141, DateTimeKind.Utc).AddTicks(1843));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 141, DateTimeKind.Utc).AddTicks(2298));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 141, DateTimeKind.Utc).AddTicks(2299));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 141, DateTimeKind.Utc).AddTicks(2301));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 141, DateTimeKind.Utc).AddTicks(2302));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 141, DateTimeKind.Utc).AddTicks(2304));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 141, DateTimeKind.Utc).AddTicks(2305));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 141, DateTimeKind.Utc).AddTicks(2306));

            migrationBuilder.UpdateData(
                schema: "jobs",
                table: "ApplicationStatuses",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 141, DateTimeKind.Utc).AddTicks(2308));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 168, DateTimeKind.Utc).AddTicks(2379));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 168, DateTimeKind.Utc).AddTicks(2385));

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 4, 3, 56, 23, 168, DateTimeKind.Utc).AddTicks(2387));
        }
    }
}
