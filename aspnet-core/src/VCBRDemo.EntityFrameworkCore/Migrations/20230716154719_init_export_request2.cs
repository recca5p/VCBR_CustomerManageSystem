using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VCBRDemo.Migrations
{
    /// <inheritdoc />
    public partial class initexportrequest2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Filter",
                table: "ExportRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Filter",
                table: "ExportRequest");
        }
    }
}
