using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchWork.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableCategoryRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "CategoryRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "CategoryRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
