using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverflowLite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTagUsageCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_UsageCount",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "UsageCount",
                table: "tags");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsageCount",
                table: "tags",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UsageCount",
                table: "tags",
                column: "UsageCount",
                descending: new bool[0]);
        }
    }
}
