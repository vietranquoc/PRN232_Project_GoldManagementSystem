function updateHeaderLoginState() {
    const token = localStorage.getItem('accessToken');
    const loginBtn = $('#login-btn');
    const profileDropdown = $('#profile-dropdown');
    if (token) {
        if (loginBtn.length) loginBtn.hide();
        if (profileDropdown.length) profileDropdown.removeClass('d-none');
    } else {
        if (loginBtn.length) loginBtn.show();
        if (profileDropdown.length) profileDropdown.addClass('d-none');
    }
    const myAccountLink = $('#my-account-link');
    if (myAccountLink.length) {
        myAccountLink.off('click').on('click', function(e) {
            e.preventDefault();
            if (token) {
                window.location.href = '../../Pages/Account/profile.html';
            } else {
                window.location.href = '../../Pages/Account/login.html';
            }
        });
    }
}

function loadProductCategoriesDropdown() {
    const ul = $('#product-category-dropdown');
    ul.html('<li><span class="dropdown-item text-muted">Đang tải...</span></li>');
    $.ajax({
        url: 'https://localhost:5000/api/Category',
        method: 'GET',
        success: function(data) {
            if (!data || data.length === 0) {
                ul.html('<li><span class="dropdown-item text-muted">Không có danh mục</span></li>');
                return;
            }
            let html = '';
            data.forEach(function(cat) {
                html += `<li><a class="dropdown-item" href="../../Pages/Product/product.html">${cat.name}</a></li>`;
            });
            ul.html(html);
        },
        error: function(xhr) {
            ul.html(`<li><span class="dropdown-item text-danger">Lỗi: ${xhr.responseText || 'Không thể lấy danh mục'}</span></li>`);
        }
    });
}

function logout() {
    Swal.fire({
        title: 'Xác nhận đăng xuất?',
        text: 'Bạn có chắc chắn muốn đăng xuất?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Đăng xuất',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            // Xóa token khỏi localStorage
            localStorage.removeItem('accessToken');
            localStorage.removeItem('userProfile');
            Swal.fire({
                icon: 'success',
                title: 'Đăng xuất thành công!',
                text: 'Bạn đã được đăng xuất khỏi hệ thống.',
                confirmButtonText: 'Đồng ý'
            }).then(() => {
                window.location.href = '../../Pages/Account/login.html';
            });
        }
    });
}

function updateCartCount() {
    const token = localStorage.getItem('accessToken') || sessionStorage.getItem('accessToken');
    if (!token) {
        $('.cart_count').text('0');
        return;
    }
    $.ajax({
        url: 'https://localhost:5000/api/cart',
        method: 'GET',
        headers: { 'Authorization': 'Bearer ' + token },
        success: function(data) {
            if (data && data.items) {
                $('.cart_count').text(data.items.length);
            } else {
                $('.cart_count').text('0');
            }
        },
        error: function() {
            $('.cart_count').text('0');
        }
    });
}

function loadHeaderFooter() {
    $(function() {
        $("#header-include").load("/Pages/Shared/header.html", function() {
            updateHeaderLoginState();
            loadProductCategoriesDropdown();
            updateCartCount(); 
        });
        $("#footer-include").load("/Pages/Shared/footer.html");
    });
} 