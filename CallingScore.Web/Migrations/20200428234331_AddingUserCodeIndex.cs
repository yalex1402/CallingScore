using Microsoft.EntityFrameworkCore.Migrations;

namespace CallingScore.Web.Migrations
{
    public partial class AddingUserCodeIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserCode",
                table: "AspNetUsers",
                column: "UserCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserCode",
                table: "AspNetUsers");
        }
    }
}
