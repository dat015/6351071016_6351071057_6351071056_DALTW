function Edit(id) {
    $('#row-display-' + id).addClass('hide');
    $('#row-edit-' + id).removeClass('hide');
}

function CancelEdit(id) {
    $('#row-edit-' + id).addClass('hide'); // Thêm lớp ẩn cho hàng chỉnh sửa
    $('#row-display-' + id).removeClass('hide'); // Gỡ lớp ẩn cho hàng hiển thị
}