using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTJ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileMissingColumns : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
