using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyManagement.Migrations
{
    public partial class AddStudentSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "ClassName", "Email", "Name" },
                values: new object[] { 1, 1, "hello1@deali.cn", "小米" });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "ClassName", "Email", "Name" },
                values: new object[] { 2, 2, "hello2@deali.cn", "华为" });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "ClassName", "Email", "Name" },
                values: new object[] { 3, 3, "hello3@deali.cn", "oppo" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
