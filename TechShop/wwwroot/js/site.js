document.addEventListener("DOMContentLoaded", function () {
    const carousel = document.querySelector('#carouselExampleCaptions');
    const items = carousel.querySelectorAll('.carousel-item');
    const buttonIndex = carousel.querySelectorAll('');
    let currentIndex = 0;
    const totalItems = items.length;

    function showSlide(index) {
        items[currentIndex].classList.remove('active');
        items[index].classList.add('active');
        currentIndex = index;
    }

    function nextSlide() {
        let nextIndex = currentIndex + 1;
        if (nextIndex >= totalItems) {
            nextIndex = 0;
        }
        showSlide(nextIndex);
    }

    setInterval(nextSlide, 3000);
});

//admin

function editItem(id) {
    document.getElementById('viewRow_' + id).style.display = 'none';
    document.getElementById('editRow_' + id).style.display = '';
}

function cancelEdit(id) {
    document.getElementById('viewRow_' + id).style.display = '';
    document.getElementById('editRow_' + id).style.display = 'none';
}





// DELETE METHOD 
function deleteItem(id) {
    if (confirm("Bạn có chắc muốn xóa mục này không?")) {
        $.ajax({
            url: '/Admin/Order/Delete',
            type: 'POST',
            data: { OrderId: id },
            success: function (result) {
                if (result.success) {
                    updateOrderList(result.orders)
                } else {
                    alert("Không tìm thấy mục cần xóa!");
                }
            },
            error: function (xhr, status, error) {
                alert("Có lỗi xảy ra!");
            }
        });
    }
}



// UPDATE METHOD 
function updateItem(id) {
    var updatedStatusPayment = document.getElementById(`statusPayment_${id}`).value;
    var updatedStatusShipping = document.getElementById(`statusShipping_${id}`).value;
    var updatedNote = document.getElementById(`note_${id}`).value;

    if (confirm("Bạn có chắc muốn cập nhật mục này không?")) {
        $.ajax({
            url: '/Admin/Order/Update',
            type: 'POST',
            data: {
                id: id,
                statusPayment: updatedStatusPayment,
                statusShipping: updatedStatusShipping,
                Note: updatedNote
            },
            success: function (result) {
                console.log(result.orders);

                if (result.success) {
                    updateOrderList(result.orders);
                } else {
                    alert("Không tìm thấy mục cần cập nhật!");
                }
            },
            error: function (xhr, status, error) {
                alert("Có lỗi xảy ra!");
            }
        });
    }
}


//LOAD TABLE 

function updateOrderList(orders) {
    var orderTableBody = $('tbody');
    if (orderTableBody.length === 0) {
        console.error('Không tìm thấy phần tử tbody.');
        return;
    }

    orderTableBody.empty();

    console.log('Danh sách đơn hàng:', orders);

    if (!orders || orders.length === 0) {
        console.warn('Không có đơn hàng nào để hiển thị.');
        return;
    }

    $.each(orders, function (index, order) {
        var newRow = $(`
            <tr id="viewRow_${order.orderId}">
                <th scope="row">${order.orderId}</th>
                <td>${order.statusPayment}</td>
                <td>${order.statusShipping}</td>
                <td>${order.note || ''}</td>
                <td>${new Date(order.orderDate).toLocaleDateString('vi-VN')}</td>
                <td>${order.paymentDate ? new Date(order.paymentDate).toLocaleDateString('vi-VN') : ''}</td>
                <td>${order.totalAmount.toLocaleString('vi-VN')} (đ)</td>
                <td scope="row">
                    <button id="edit" type="button" class="btn btn-secondary" onclick="editItem(${order.orderId})">
                        <i class="bi bi-pencil-square"></i> Sửa
                    </button>
                    <button id="delete" type="button" class="btn btn-danger" onclick="deleteItem(${order.orderId})">
                        <i class="bi bi-trash"></i> Xóa
                    </button>
                </td>
            </tr>
               <tr id="editRow_${order.orderId}" style="display:none;">
                <th scope="row">${order.orderId}</th>
                <td>
                    <input type="text" id="statusPayment_${order.orderId}" value="${order.statusPayment}" class="form-control">
                </td>
                <td>
                    <input type="text" id="statusShipping_${order.orderId}" value="${order.statusShipping}" class="form-control">
                </td>
                <td>
                    <input type="text" id="note_${order.orderId}" value="${order.note}" class="form-control">
                </td>
                <td>
                    <input type="text" id="orderDate_${order.orderId}" value="${new Date(order.orderDate).toLocaleDateString('vi-VN')}" class="form-control" disabled>
                </td>
                <td>
                    <input type="text" id="paymentDate_${order.orderId}" value="${order.paymentDate ? new Date(order.paymentDate).toLocaleDateString('vi-VN') : ''}" class="form-control" disabled>
                </td>
                <td>
                    <input type="text" value="${order.totalAmount.toLocaleString('vi-VN')} (đ)" class="form-control" disabled>
                </td>
                <td scope="col">
                    <button type="button" id="save" class="btn btn-secondary" onclick="updateItem(@item.OrderId)">
                        <i class="bi bi-save"></i> Lưu
                    </button>
                    <button type="button" id="cancel" class="btn btn-danger" onclick="cancelEdit(@item.OrderId)">
                        <i class="bi bi-x-circle"></i> Hủy
                    </button>
                </td>
            </tr>
        `);

        orderTableBody.append(newRow);
      

    });
}




