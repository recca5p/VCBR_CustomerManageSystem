using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VCBRDemo.Migrations
{
    /// <inheritdoc />
    public partial class updateexportrequestentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "ExportRequest");

            migrationBuilder.DropColumn(
                name: "Filename",
                table: "ExportRequest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "ExportRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Filename",
                table: "ExportRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
