$(document).ready(function () {
    // Gắn sự kiện click cho từng item
    $(".config-item").on("click", function () {
        const configId = $(this).data("configid"); // Lấy ConfigId từ thuộc tính data
        const productId = $(this).data("productid"); // Lấy ProductId từ thuộc tính data

        // Gọi AJAX để xử lý cấu hình được chọn
        pickConfig(productId, configId);

        // Cập nhật giao diện: đánh dấu item được chọn
        $(".config-item").css("background", "#dddddd3d"); // Reset background
        $(this).css("background", "#cce5ff"); // Highlight item được chọn
    });
});

function pickConfig(ProductId, ConfigId) {
    $.ajax({
        url: "/Customer/Home/ProductDetail",
        type: "GET",
        data: {
            id: ProductId,
            configId: ConfigId
        },
        beforeSend: function () {
            console.log("Đang tải dữ liệu...");
        },
        success: function (response) {
            console.log("Cấu hình đã được chọn thành công.");
            // Có thể cập nhật giao diện khác hoặc hiển thị thông báo
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi chọn cấu hình: ", error);
            alert("Đã xảy ra lỗi khi chọn cấu hình. Vui lòng thử lại!");
        }
    });
}
