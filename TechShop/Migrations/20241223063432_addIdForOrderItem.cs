using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechShop.Migrations
{
    /// <inheritdoc />
    public partial class addIdForOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
       name: "Id",
       table: "DetailsOrders",
       type: "int",
       nullable: false,
       defaultValue: 0)
       .Annotation("SqlServer:Identity", "1, 1"); // Bật Identity (tự động tăng)

            // Thiết lập "Id" làm khóa chính
            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailsOrders",
                table: "DetailsOrders",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
          name: "PK_DetailsOrders",
          table: "DetailsOrders");

            // Xóa cột Id
            migrationBuilder.DropColumn(
                name: "Id",
                table: "DetailsOrders");
        }
    }
}
