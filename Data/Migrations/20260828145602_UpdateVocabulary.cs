using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeThongHocNgoaiNguTrucTuyen.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVocabulary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phoenic",
                table: "Vocabularies",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Phoenic",
                table: "Vocabularies");
        }
    }
}
