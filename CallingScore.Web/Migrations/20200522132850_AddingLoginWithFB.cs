using Microsoft.EntityFrameworkCore.Migrations;

namespace CallingScore.Web.Migrations
{
    public partial class AddingLoginWithFB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginType",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginType",
                table: "AspNetUsers");
        }
    }
}
