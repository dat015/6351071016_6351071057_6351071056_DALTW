using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechShop.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompositeKeyFromCartDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            // Thêm cột Id vào bảng CartDetails
            migrationBuilder.AddColumn<int>(
             name: "Id",
             table: "CartDetails",
             nullable: false,
             defaultValue: 0)
             .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);


            // Thêm khóa chính mới cho cột Id
            migrationBuilder.AddPrimaryKey(
                name: "PK_CartDetails",
                table: "CartDetails",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa khóa chính mới
            migrationBuilder.DropPrimaryKey(
                name: "PK_CartDetails",
                table: "CartDetails");

            // Xóa cột Id
            migrationBuilder.DropColumn(
                name: "Id",
                table: "CartDetails");

            // Khôi phục khóa chính cũ (nếu cần)
        }

    }
}
