using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VCBRDemo.Migrations
{
    /// <inheritdoc />
    public partial class importrequestupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReportLink",
                table: "ImportRequest",
                newName: "ReportId");

            migrationBuilder.RenameColumn(
                name: "FileLink",
                table: "ImportRequest",
                newName: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReportId",
                table: "ImportRequest",
                newName: "ReportLink");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "ImportRequest",
                newName: "FileLink");
        }
    }
}
