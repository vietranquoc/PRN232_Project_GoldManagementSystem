
// Kiểm tra trạng thái đăng nhập khi trang được tải
document.addEventListener('DOMContentLoaded', function() {
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập! Vui lòng đăng nhập để tiếp tục.',
        confirmButtonText: 'Đồng ý'
    }).then(() => {
        window.location.href = '../Account/login.html';
    });
    return;
    }
});

// Chuyển đổi section khi click sidebar
document.getElementById('sidebar-dashboard').addEventListener('click', function(e) {
    e.preventDefault();
    document.getElementById('dashboard-section').style.display = '';
    document.getElementById('account-section').style.display = 'none';
    document.getElementById('category-section').style.display = 'none';
    document.getElementById('product-section').style.display = 'none';
    document.getElementById('goldtype-section').style.display = 'none';
    document.getElementById('goldprice-section').style.display = 'none';
    setActiveSidebar(this);
});
document.getElementById('sidebar-account').addEventListener('click', function(e) {
    console.log('Clicked Account tab');
    e.preventDefault();
    document.getElementById('dashboard-section').style.display = 'none';
    document.getElementById('account-section').style.display = '';
    document.getElementById('category-section').style.display = 'none';
    document.getElementById('product-section').style.display = 'none';
    document.getElementById('goldtype-section').style.display = 'none';
    document.getElementById('goldprice-section').style.display = 'none';
    setActiveSidebar(this);
    loadAccounts();
});
document.getElementById('sidebar-category').addEventListener('click', function(e) {
    console.log('Clicked Category tab');
    e.preventDefault();
    document.getElementById('dashboard-section').style.display = 'none';
    document.getElementById('account-section').style.display = 'none';
    document.getElementById('category-section').style.display = '';
    document.getElementById('product-section').style.display = 'none';
    document.getElementById('goldtype-section').style.display = 'none';
    document.getElementById('goldprice-section').style.display = 'none';
    setActiveSidebar(this);
    loadCategories();
});
document.getElementById('sidebar-product').addEventListener('click', function(e) {
    e.preventDefault();
    document.getElementById('dashboard-section').style.display = 'none';
    document.getElementById('account-section').style.display = 'none';
    document.getElementById('category-section').style.display = 'none';
    document.getElementById('product-section').style.display = '';
    document.getElementById('goldtype-section').style.display = 'none';
    document.getElementById('goldprice-section').style.display = 'none';
    setActiveSidebar(this);
    loadProducts();
});
// Sidebar chuyển tab GoldType/GoldPrice
document.getElementById('sidebar-goldtype').addEventListener('click', function(e) {
    e.preventDefault();
    document.getElementById('dashboard-section').style.display = 'none';
    document.getElementById('account-section').style.display = 'none';
    document.getElementById('category-section').style.display = 'none';
    document.getElementById('product-section').style.display = 'none';
    document.getElementById('goldtype-section').style.display = '';
    document.getElementById('goldprice-section').style.display = 'none';
    setActiveSidebar(this);
    loadGoldTypes();
});
document.getElementById('sidebar-goldprice').addEventListener('click', function(e) {
    e.preventDefault();
    document.getElementById('dashboard-section').style.display = 'none';
    document.getElementById('account-section').style.display = 'none';
    document.getElementById('category-section').style.display = 'none';
    document.getElementById('product-section').style.display = 'none';
    document.getElementById('goldtype-section').style.display = 'none';
    document.getElementById('goldprice-section').style.display = '';
    setActiveSidebar(this);
    loadGoldTypes();
});
function setActiveSidebar(activeLink) {
    document.querySelectorAll('.sidebar .nav-link').forEach(function(link) {
    link.classList.remove('active');
    });
    activeLink.classList.add('active');
}

// Account Management Functions
function loadAccounts() {
    console.log('Start loadAccounts');
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    $.ajax({
    url: 'https://localhost:5000/api/Account/all',
    method: 'GET',
    headers: { 
        'Authorization': 'Bearer ' + token 
    },
    success: function(accounts) {
        console.log('API /api/Account/all response:', accounts);
        renderAccounts(accounts);
    },
    error: function(xhr) {
        console.log('API /api/Account/all error:', xhr.status, xhr.responseText);
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Không thể lấy danh sách tài khoản! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
        });
    }
    });
}

function renderAccounts(accounts) {
    console.log('Render accounts:', accounts);
    const tbody = document.getElementById('accountsTableBody');
    tbody.innerHTML = '';
    accounts.forEach(acc => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
        <td>${acc.id}</td>
        <td>${acc.username}</td>
        <td>${acc.email}</td>
        <td>${acc.fullName}</td>
        <td><span class="badge ${acc.roleName === 'Manager' ? 'bg-primary' : acc.roleName === 'Employee' ? 'bg-info' : 'bg-secondary'}">${acc.roleName}</span></td>
        <td><span class="badge ${acc.isActive ? 'bg-success' : 'bg-warning text-dark'}">${acc.isActive ? 'Active' : 'Inactive'}</span></td>
        <td>${acc.createdDate ? acc.createdDate.split('T')[0] : ''}</td>
        <td></td>
    `;
    tbody.appendChild(tr);
    });
    filterAccounts();
}

function createAccount() {
    const form = document.getElementById('createAccountForm');
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    // Validate password confirmation
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
    if (password !== confirmPassword) {
    Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Mật khẩu và xác nhận mật khẩu không khớp!',
        confirmButtonText: 'Đóng'
    });
    return;
    }
    // Lấy role
    const role = document.getElementById('role').value;
    if (role !== 'Employee' && role !== 'Nhân viên') {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Chỉ tạo được tài khoản nhân viên từ đây!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    const dto = {
    username: document.getElementById('username').value,
    password: password,
    confirmPassword: confirmPassword,
    fullName: document.getElementById('fullName').value,
    phone: document.getElementById('phone').value,
    address: document.getElementById('address').value,
    email: document.getElementById('email').value
    };
    $.ajax({
    url: 'https://localhost:5000/api/Account/create-employee',
    type: 'POST',
    headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    },
    data: JSON.stringify(dto),
    success: function() {
        const modal = bootstrap.Modal.getInstance(document.getElementById('createAccountModal'));
        modal.hide();
        Swal.fire({
        icon: 'success',
        title: 'Thành công!',
        text: 'Tài khoản nhân viên đã được tạo thành công.',
        confirmButtonText: 'Đồng ý'
        });
        form.reset();
        loadAccounts();
    },
    error: function(xhr) {
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Tạo tài khoản thất bại! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
        });
    }
    });
}

function editAccount(accountId) {
    // Here you would typically fetch account data from your API
    // For demo purposes, we'll populate with sample data
    document.getElementById('editAccountId').value = accountId;
    document.getElementById('editUsername').value = 'john.doe';
    document.getElementById('editEmail').value = 'john.doe@example.com';
    document.getElementById('editFullName').value = 'John Doe';
    document.getElementById('editPhone').value = '+1234567890';
    document.getElementById('editRole').value = 'Manager';
    document.getElementById('editStatus').value = 'Active';
    document.getElementById('editAddress').value = '123 Main St, City, State';
    
    // Show the edit modal
    const editModal = new bootstrap.Modal(document.getElementById('editAccountModal'));
    editModal.show();
}

function updateAccount() {
    // Placeholder: cần endpoint update ở backend
    Swal.fire({
    icon: 'info',
    title: 'Thông báo!',
    text: 'Chức năng cập nhật tài khoản cần bổ sung endpoint API.',
    confirmButtonText: 'Đồng ý'
    });
}

function deleteAccount(accountId) {
    // Placeholder: cần endpoint xóa ở backend
    Swal.fire({
    icon: 'info',
    title: 'Thông báo!',
    text: 'Chức năng xóa tài khoản cần bổ sung endpoint API.',
    confirmButtonText: 'Đồng ý'
    });
}

// Filter and search functionality
document.getElementById('roleFilter').addEventListener('change', filterAccounts);
document.getElementById('statusFilter').addEventListener('change', filterAccounts);
document.getElementById('searchAccount').addEventListener('input', filterAccounts);

function filterAccounts() {
    const roleFilter = document.getElementById('roleFilter').value;
    const statusFilter = document.getElementById('statusFilter').value;
    const searchTerm = document.getElementById('searchAccount').value.toLowerCase();
    
    const rows = document.querySelectorAll('#accountsTableBody tr');
    
    rows.forEach(row => {
    const role = row.cells[4].textContent.trim();
    const status = row.cells[5].textContent.trim();
    const name = row.cells[3].textContent.toLowerCase();
    const email = row.cells[2].textContent.toLowerCase();
    
    const matchesRole = !roleFilter || role.includes(roleFilter);
    const matchesStatus = !statusFilter || status.includes(statusFilter);
    const matchesSearch = !searchTerm || name.includes(searchTerm) || email.includes(searchTerm);
    
    row.style.display = matchesRole && matchesStatus && matchesSearch ? '' : 'none';
    });
}

// Clear form when modal is closed
document.getElementById('createAccountModal').addEventListener('hidden.bs.modal', function () {
    document.getElementById('createAccountForm').reset();
});

// Category Management Functions
function loadCategories() {
    console.log('Start loadCategories');
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    $.ajax({
    url: 'https://localhost:5000/api/Category',
    type: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(categories) {
        console.log('API /api/Category response:', categories);
        renderCategories(categories);
    },
    error: function(xhr) {
        console.log('API /api/Category error:', xhr.status, xhr.responseText);
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Không thể lấy danh sách danh mục! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
        });
    }
    });
}

function renderCategories(categories) {
    console.log('Render categories:', categories);
    const tbody = document.getElementById('categoriesTableBody');
    tbody.innerHTML = '';
    categories.forEach(cat => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
        <td>${cat.id}</td>
        <td>${cat.name}</td>
        <td>${cat.description || ''}</td>
        <td><span class="badge ${cat.isActive ? 'bg-success' : 'bg-warning text-dark'}">${cat.isActive ? 'Đang hoạt động' : 'Ngừng hoạt động'}</span></td>
        <td>${cat.createdDate ? cat.createdDate.split('T')[0] : ''}</td>
        <td>
        <button class="btn btn-sm btn-outline-primary me-1" onclick="editCategory(${cat.id})">
            <i class="fas fa-edit"></i>
        </button>
        <button class="btn btn-sm btn-outline-danger" onclick="deleteCategory(${cat.id})">
            <i class="fas fa-trash"></i>
        </button>
        </td>
    `;
    tbody.appendChild(tr);
    });
    filterCategories();
}

function createCategory() {
    const form = document.getElementById('createCategoryForm');
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    const dto = {
    name: document.getElementById('categoryName').value,
    description: document.getElementById('categoryDescription').value
    };
    $.ajax({
    url: 'https://localhost:5000/api/Category',
    type: 'POST',
    headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    },
    data: JSON.stringify(dto),
    success: function() {
        const modal = bootstrap.Modal.getInstance(document.getElementById('createCategoryModal'));
        modal.hide();
        Swal.fire({
        icon: 'success',
        title: 'Thành công!',
        text: 'Danh mục đã được tạo thành công.',
        confirmButtonText: 'Đồng ý'
        });
        form.reset();
        loadCategories();
    },
    error: function(xhr) {
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Tạo danh mục thất bại! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
        });
    }
    });
}

function editCategory(categoryId) {
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    $.ajax({
    url: `https://localhost:5000/api/Category/${categoryId}`,
    type: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(category) {
        document.getElementById('editCategoryId').value = category.id;
        document.getElementById('editCategoryName').value = category.name;
        document.getElementById('editCategoryDescription').value = category.description || '';
        document.getElementById('editCategoryStatus').value = category.isActive ? 'Active' : 'Inactive';
        
        const editModal = new bootstrap.Modal(document.getElementById('editCategoryModal'));
        editModal.show();
    },
    error: function(xhr) {
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Không thể lấy thông tin danh mục! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
        });
    }
    });
}

function updateCategory() {
    const form = document.getElementById('editCategoryForm');
    const categoryId = document.getElementById('editCategoryId').value;
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    const dto = {
    name: document.getElementById('editCategoryName').value,
    description: document.getElementById('editCategoryDescription').value,
    isActive: document.getElementById('editCategoryStatus').value === 'Active'
    };
    $.ajax({
    url: `https://localhost:5000/api/Category/${categoryId}`,
    type: 'PUT',
    headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    },
    data: JSON.stringify(dto),
    success: function() {
        const modal = bootstrap.Modal.getInstance(document.getElementById('editCategoryModal'));
        modal.hide();
        Swal.fire({
        icon: 'success',
        title: 'Thành công!',
        text: 'Danh mục đã được cập nhật thành công.',
        confirmButtonText: 'Đồng ý'
        });
        loadCategories();
    },
    error: function(xhr) {
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Cập nhật danh mục thất bại! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
        });
    }
    });
}

function deleteCategory(categoryId) {
    Swal.fire({
    title: 'Xác nhận xóa?',
    text: 'Bạn có chắc chắn muốn xóa danh mục này?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#d33',
    cancelButtonColor: '#3085d6',
    confirmButtonText: 'Xóa',
    cancelButtonText: 'Hủy'
    }).then((result) => {
    if (result.isConfirmed) {
        const token = localStorage.getItem('accessToken');
        if (!token) {
        Swal.fire({
            icon: 'warning',
            title: 'Cảnh báo!',
            text: 'Bạn chưa đăng nhập!',
            confirmButtonText: 'Đồng ý'
        });
        return;
        }
        $.ajax({
        url: `https://localhost:5000/api/Category/${categoryId}`,
        type: 'DELETE',
        headers: { 'Authorization': 'Bearer ' + token },
        success: function() {
            Swal.fire({
            icon: 'success',
            title: 'Thành công!',
            text: 'Danh mục đã được xóa thành công.',
            confirmButtonText: 'Đồng ý'
            });
            loadCategories();
        },
        error: function(xhr) {
            Swal.fire({
            icon: 'error',
            title: 'Lỗi!',
            text: 'Xóa danh mục thất bại! ' + (xhr.responseText || ''),
            confirmButtonText: 'Đóng'
            });
        }
        });
    }
    });
}

// Filter and search functionality for categories
document.getElementById('searchCategory').addEventListener('input', filterCategories);
document.getElementById('statusCategoryFilter').addEventListener('change', filterCategories);

function filterCategories() {
    const statusFilter = document.getElementById('statusCategoryFilter').value;
    const searchTerm = document.getElementById('searchCategory').value.toLowerCase();
    
    const rows = document.querySelectorAll('#categoriesTableBody tr');
    
    rows.forEach(row => {
    const status = row.cells[3].textContent.trim();
    const name = row.cells[1].textContent.toLowerCase();
    const description = row.cells[2].textContent.toLowerCase();
    
    const matchesStatus = !statusFilter || status.includes(statusFilter);
    const matchesSearch = !searchTerm || name.includes(searchTerm) || description.includes(searchTerm);
    
    row.style.display = matchesStatus && matchesSearch ? '' : 'none';
    });
}

// Clear form when category modal is closed
document.getElementById('createCategoryModal').addEventListener('hidden.bs.modal', function () {
    document.getElementById('createCategoryForm').reset();
});

// Product Management Functions
async function loadProducts() {
    console.log('Start loadProducts');
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    // Load both products and categories, then map category name
    try {
    const [products, categories] = await Promise.all([
        $.ajax({
        url: 'https://localhost:5000/api/Product',
        type: 'GET',
        headers: { 'Authorization': 'Bearer ' + token }
        }),
        $.ajax({
        url: 'https://localhost:5000/api/Category',
        type: 'GET',
        headers: { 'Authorization': 'Bearer ' + token }
        })
    ]);
    // Map categoryId to name
    const catMap = {};
    categories.forEach(cat => { catMap[cat.id] = cat.name; });
    products.forEach(prod => {
        prod.categoryName = catMap[prod.categoryId] || '';
    });
    renderProducts(products);
    } catch (xhr) {
    Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Không thể lấy danh sách sản phẩm hoặc danh mục!',
        confirmButtonText: 'Đóng'
    });
    }
}

function renderProducts(products) {
    console.log('Render products:', products);
    const tbody = document.getElementById('productsTableBody');
    tbody.innerHTML = '';
    products.forEach(prod => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
        <td>${prod.id}</td>
        <td>${prod.name}</td>
        <td>${prod.categoryName || ''}</td>
        <td>${prod.description || ''}</td>
        <td><span class="badge ${prod.isActive ? 'bg-success' : 'bg-warning text-dark'}">${prod.isActive ? 'Đang hoạt động' : 'Ngừng hoạt động'}</span></td>
        <td>${prod.createdDate ? prod.createdDate.split('T')[0] : ''}</td>
        <td>
        <button class="btn btn-sm btn-outline-primary me-1" onclick="editProduct(${prod.id})">
            <i class="fas fa-edit"></i>
        </button>
        <button class="btn btn-sm btn-outline-danger" onclick="deleteProduct(${prod.id})">
            <i class="fas fa-trash"></i>
        </button>
        </td>
    `;
    tbody.appendChild(tr);
    });
    filterProducts();
}

async function createProduct() {
    const form = document.getElementById('createProductForm');
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    
    const dto = {
    name: document.getElementById('productName').value,
    description: document.getElementById('productDescription').value,
    categoryId: parseInt(document.getElementById('productCategory').value),
    goldTypeId: parseInt(document.getElementById('productGoldType').value),
    price: parseFloat(document.getElementById('productPrice').value),
    quantity: parseInt(document.getElementById('productQuantity').value)
    };
    
    try {
    // Tạo sản phẩm trước
    const productResponse = await $.ajax({
        url: 'https://localhost:5000/api/Product',
        type: 'POST',
        headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
        },
        data: JSON.stringify(dto)
    });
    
    const productId = productResponse;
    
    // Upload ảnh nếu có
    const files = document.getElementById('productImages').files;
    if (files.length > 0) {
        await uploadMultipleProductImages(productId, files, token);
    }
    
    const modal = bootstrap.Modal.getInstance(document.getElementById('createProductModal'));
    modal.hide();
    Swal.fire({
        icon: 'success',
        title: 'Thành công!',
        text: 'Sản phẩm đã được tạo thành công.',
        confirmButtonText: 'Đồng ý'
    });
    form.reset();
    loadProducts();
    } catch (xhr) {
    Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Tạo sản phẩm thất bại! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
    });
    }
}
async function uploadMultipleProductImages(productId, files, token) {
    // Tạo danh sách ảnh để gửi
    const images = [];
    for (let i = 0; i < files.length; i++) {
    const file = files[i];
    // Convert file to base64 hoặc upload lên server trước
    const imageUrl = await convertFileToBase64(file);
    images.push({
        imageUrl: imageUrl,
        isMain: i === 0 // Ảnh đầu tiên là ảnh chính
    });
    }
    
    // Gọi API để tạo nhiều ảnh cùng lúc
    const dto = {
    productId: productId,
    images: images
    };
    
    return await $.ajax({
    url: 'https://localhost:5000/api/ProductImage/multiple',
    type: 'POST',
    headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    },
    data: JSON.stringify(dto)
    });
}

function convertFileToBase64(file) {
    return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
    });
}

// Function để preview ảnh
function previewImages(input) {
    const preview = document.getElementById('imagePreview');
    preview.innerHTML = '';
    
    if (input.files && input.files.length > 0) {
    for (let i = 0; i < input.files.length; i++) {
        const file = input.files[i];
        const reader = new FileReader();
        
        reader.onload = function(e) {
        const div = document.createElement('div');
        div.className = 'position-relative';
        div.innerHTML = `
            <img src="${e.target.result}" class="img-thumbnail" style="width: 100px; height: 100px; object-fit: cover;" alt="Preview">
            <div class="position-absolute top-0 start-0 bg-primary text-white px-1 py-0 rounded" style="font-size: 0.7rem;">
            ${i === 0 ? 'Chính' : i + 1}
            </div>
        `;
        preview.appendChild(div);
        };
        
        reader.readAsDataURL(file);
    }
    }
}

// Giữ lại function cũ để tương thích ngược
function uploadProductImage(productId, file, isMain, token) {
    const formData = new FormData();
    formData.append('productId', productId);
    formData.append('image', file);
    formData.append('isMain', isMain);
    $.ajax({
    url: 'https://localhost:5000/api/ProductImage',
    type: 'POST',
    headers: { 'Authorization': 'Bearer ' + token },
    data: formData,
    processData: false,
    contentType: false
    });
}

function editProduct(productId) {
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    // Load product, categories, gold types, rồi fill modal
    Promise.all([
    $.ajax({
        url: `https://localhost:5000/api/Product/${productId}`,
        type: 'GET',
        headers: { 'Authorization': 'Bearer ' + token }
    }),
    $.ajax({
        url: 'https://localhost:5000/api/Category',
        type: 'GET',
        headers: { 'Authorization': 'Bearer ' + token }
    }),
    $.ajax({
        url: 'https://localhost:5000/api/GoldType',
        type: 'GET',
        headers: { 'Authorization': 'Bearer ' + token }
    })
    ]).then(([product, categories, goldTypes]) => {
    document.getElementById('editProductId').value = product.id;
    document.getElementById('editProductName').value = product.name;
    document.getElementById('editProductDescription').value = product.description || '';
    document.getElementById('editProductStatus').value = product.isActive ? 'Active' : 'Inactive';
    // Fill category select
    const selectCat = document.getElementById('editProductCategory');
    selectCat.innerHTML = '';
    categories.forEach(cat => {
        const option = document.createElement('option');
        option.value = cat.id;
        option.textContent = cat.name;
        if (product.categoryId === cat.id) option.selected = true;
        selectCat.appendChild(option);
    });
    // Fill gold type select
    const selectGold = document.getElementById('editProductGoldType');
    selectGold.innerHTML = '';
    goldTypes.forEach(gt => {
        const option = document.createElement('option');
        option.value = gt.id;
        option.textContent = gt.name;
        if (product.goldTypeId === gt.id) option.selected = true;
        selectGold.appendChild(option);
    });
    document.getElementById('editProductPrice').value = product.price;
    document.getElementById('editProductQuantity').value = product.quantity;
    const editModal = new bootstrap.Modal(document.getElementById('editProductModal'));
    editModal.show();
    }).catch(xhr => {
    Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Không thể lấy thông tin sản phẩm, danh mục hoặc loại vàng!',
        confirmButtonText: 'Đóng'
    });
    });
}

function updateProduct() {
    const form = document.getElementById('editProductForm');
    const productId = document.getElementById('editProductId').value;
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    const dto = {
    id: parseInt(productId),
    name: document.getElementById('editProductName').value,
    description: document.getElementById('editProductDescription').value,
    categoryId: parseInt(document.getElementById('editProductCategory').value),
    goldTypeId: parseInt(document.getElementById('editProductGoldType').value),
    price: parseFloat(document.getElementById('editProductPrice').value),
    quantity: parseInt(document.getElementById('editProductQuantity').value),
    isActive: document.getElementById('editProductStatus').value === 'Active'
    };
    $.ajax({
    url: `https://localhost:5000/api/Product/${productId}`,
    type: 'PUT',
    headers: {
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
    },
    data: JSON.stringify(dto),
    success: function() {
        const modal = bootstrap.Modal.getInstance(document.getElementById('editProductModal'));
        modal.hide();
        Swal.fire({
        icon: 'success',
        title: 'Thành công!',
        text: 'Sản phẩm đã được cập nhật thành công.',
        confirmButtonText: 'Đồng ý'
        });
        loadProducts();
    },
    error: function(xhr) {
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Cập nhật sản phẩm thất bại! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
        });
    }
    });
}

function deleteProduct(productId) {
    Swal.fire({
    title: 'Xác nhận xóa?',
    text: 'Bạn có chắc chắn muốn xóa sản phẩm này?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#d33',
    cancelButtonColor: '#3085d6',
    confirmButtonText: 'Xóa',
    cancelButtonText: 'Hủy'
    }).then((result) => {
    if (result.isConfirmed) {
        const token = localStorage.getItem('accessToken');
        if (!token) {
        Swal.fire({
            icon: 'warning',
            title: 'Cảnh báo!',
            text: 'Bạn chưa đăng nhập!',
            confirmButtonText: 'Đồng ý'
        });
        return;
        }
        $.ajax({
        url: `https://localhost:5000/api/Product/${productId}`,
        type: 'DELETE',
        headers: { 'Authorization': 'Bearer ' + token },
        success: function() {
            Swal.fire({
            icon: 'success',
            title: 'Thành công!',
            text: 'Sản phẩm đã được xóa thành công.',
            confirmButtonText: 'Đồng ý'
            });
            loadProducts();
        },
        error: function(xhr) {
            Swal.fire({
            icon: 'error',
            title: 'Lỗi!',
            text: 'Xóa sản phẩm thất bại! ' + (xhr.responseText || ''),
            confirmButtonText: 'Đóng'
            });
        }
        });
    }
    });
}

// Filter and search functionality for products
document.getElementById('searchProduct').addEventListener('input', filterProducts);
document.getElementById('statusProductFilter').addEventListener('change', filterProducts);

function filterProducts() {
    const statusFilter = document.getElementById('statusProductFilter').value;
    const searchTerm = document.getElementById('searchProduct').value.toLowerCase();
    const rows = document.querySelectorAll('#productsTableBody tr');
    rows.forEach(row => {
    const status = row.cells[4].textContent.trim();
    const name = row.cells[1].textContent.toLowerCase();
    const description = row.cells[3].textContent.toLowerCase();
    const matchesStatus = !statusFilter || status.includes(statusFilter);
    const matchesSearch = !searchTerm || name.includes(searchTerm) || description.includes(searchTerm);
    row.style.display = matchesStatus && matchesSearch ? '' : 'none';
    });
}

// Load categories for product create/edit modal
function loadCategoriesForProductSelect(selectedId) {
    const token = localStorage.getItem('accessToken');
    if (!token) return;
    $.ajax({
    url: 'https://localhost:5000/api/Category',
    type: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(categories) {
        const select = document.getElementById('productCategory');
        select.innerHTML = '';
        categories.forEach(cat => {
        const option = document.createElement('option');
        option.value = cat.id;
        option.textContent = cat.name;
        if (selectedId && cat.id == selectedId) option.selected = true;
        select.appendChild(option);
        });
    }
    });
}
function loadCategoriesForProductEdit(selectedId) {
    const token = localStorage.getItem('accessToken');
    if (!token) return;
    $.ajax({
    url: 'https://localhost:5000/api/Category',
    type: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(categories) {
        const select = document.getElementById('editProductCategory');
        select.innerHTML = '';
        categories.forEach(cat => {
        const option = document.createElement('option');
        option.value = cat.id;
        option.textContent = cat.name;
        if (selectedId && cat.id == selectedId) option.selected = true;
        select.appendChild(option);
        });
    }
    });
}
// Clear form when product modal is closed
document.getElementById('createProductModal').addEventListener('hidden.bs.modal', function () {
    document.getElementById('createProductForm').reset();
    document.getElementById('imagePreview').innerHTML = '';
});
// Load categories when open create product modal
document.getElementById('createProductModal').addEventListener('show.bs.modal', function () {
    loadCategoriesForProductSelect();
    loadGoldTypesForProductSelect();
});
function loadGoldTypesForProductSelect(selectedId) {
    const token = localStorage.getItem('accessToken');
    if (!token) return;
    $.ajax({
    url: 'https://localhost:5000/api/GoldType',
    type: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(goldTypes) {
        const select = document.getElementById('productGoldType');
        select.innerHTML = '';
        goldTypes.forEach(gt => {
        const option = document.createElement('option');
        option.value = gt.id;
        option.textContent = gt.name;
        if (selectedId && gt.id == selectedId) option.selected = true;
        select.appendChild(option);
        });
    }
    });
}

// ========== GOLD TYPE ==========
function loadGoldTypes() {
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    
    $.ajax({
    url: 'https://localhost:5000/api/GoldType',
    type: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(data) {
        const tbody = $('#goldtype-table tbody');
        tbody.empty();
        data.forEach(gt => {
        tbody.append(`
            <tr>
            <td>${gt.name}</td>
            <td>${gt.description || ''}</td>
            <td>${gt.karat}</td>
            <td>${gt.priceType}</td>
            <td>${gt.isActive ? 'Hoạt động' : 'Ngừng'}</td>
            <td>${gt.createdDate ? gt.createdDate.split('T')[0] : ''}</td>
            <td>
                <button class="btn btn-sm btn-outline-primary me-1" onclick="showEditGoldTypeModal(${gt.id})"><i class="fas fa-edit"></i></button>
                <button class="btn btn-sm btn-outline-danger" onclick="deleteGoldType(${gt.id})"><i class="fas fa-trash"></i></button>
            </td>
            </tr>
        `);
        });
        // Load cho filter GoldPrice
        loadGoldTypeOptions(data);
    },
    error: function(xhr) {
        console.error('Error loading gold types:', xhr);
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Không thể tải danh sách loại vàng! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
        });
    }
    });
}
function showCreateGoldTypeModal() {
    $('#goldTypeModalTitle').text('Thêm loại vàng');
    $('#goldTypeForm')[0].reset();
    $('#goldTypeId').val('');
    $('#goldTypeActiveGroup').hide();
    $('#goldTypeModal').modal('show');
}
function showEditGoldTypeModal(id) {
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    
    $.ajax({
    url: `https://localhost:5000/api/GoldType/${id}`,
    type: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(gt) {
        $('#goldTypeModalTitle').text('Sửa loại vàng');
        $('#goldTypeId').val(gt.id);
        $('#goldTypeName').val(gt.name);
        $('#goldTypeDescription').val(gt.description);
        $('#goldTypeKarat').val(gt.karat);
        $('#goldTypePriceType').val(gt.priceType);
        $('#goldTypeIsActive').val(gt.isActive ? 'true' : 'false');
        $('#goldTypeActiveGroup').show();
        $('#goldTypeModal').modal('show');
    },
    error: function(xhr) {
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Không thể lấy thông tin loại vàng! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
        });
    }
    });
}
$('#goldTypeForm').off('submit').on('submit', function(e) {
    e.preventDefault();
    const id = $('#goldTypeId').val();
    const dto = {
    name: $('#goldTypeName').val(),
    description: $('#goldTypeDescription').val(),
    karat: $('#goldTypeKarat').val() ? parseInt($('#goldTypeKarat').val()) : null,
    priceType: $('#goldTypePriceType').val()
    };
    if (id) {
    dto.id = parseInt(id);
    dto.isActive = $('#goldTypeIsActive').val() === 'true';
    $.ajax({
        url: `https://localhost:5000/api/GoldType/${id}`,
        type: 'PUT',
        headers: {
        'Authorization': 'Bearer ' + localStorage.getItem('accessToken'),
        'Content-Type': 'application/json'
        },
        data: JSON.stringify(dto),
        success: function() {
        Swal.fire({
            icon: 'success',
            title: 'Thành công!',
            text: 'Cập nhật loại vàng thành công.',
            confirmButtonText: 'Đồng ý'
        }).then(() => {
            $('#goldTypeModal').modal('hide');
            loadGoldTypes();
        });
        },
        error: function(xhr) {
        Swal.fire({
            icon: 'error',
            title: 'Lỗi!',
            text: 'Cập nhật loại vàng thất bại! ' + (xhr.responseText || ''),
            confirmButtonText: 'Đóng'
        });
        }
    });
    } else {
    $.ajax({
        url: 'https://localhost:5000/api/GoldType',
        type: 'POST',
        headers: {
        'Authorization': 'Bearer ' + localStorage.getItem('accessToken'),
        'Content-Type': 'application/json'
        },
        data: JSON.stringify(dto),
        success: function() {
        Swal.fire({
            icon: 'success',
            title: 'Thành công!',
            text: 'Thêm loại vàng thành công.',
            confirmButtonText: 'Đồng ý'
        }).then(() => {
            $('#goldTypeModal').modal('hide');
            loadGoldTypes();
        });
        },
        error: function(xhr) {
        Swal.fire({
            icon: 'error',
            title: 'Lỗi!',
            text: 'Tạo loại vàng thất bại! ' + (xhr.responseText || ''),
            confirmButtonText: 'Đóng'
        });
        }
    });
    }
});
function deleteGoldType(id) {
    Swal.fire({
    title: 'Xác nhận xóa?',
    text: 'Bạn có chắc chắn muốn xóa loại vàng này?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#d33',
    cancelButtonColor: '#3085d6',
    confirmButtonText: 'Xóa',
    cancelButtonText: 'Hủy'
    }).then((result) => {
    if (result.isConfirmed) {
        const token = localStorage.getItem('accessToken');
        if (!token) {
        Swal.fire({
            icon: 'warning',
            title: 'Cảnh báo!',
            text: 'Bạn chưa đăng nhập!',
            confirmButtonText: 'Đồng ý'
        });
        return;
        }
        
        $.ajax({
        url: `https://localhost:5000/api/GoldType/${id}`,
        type: 'DELETE',
        headers: { 'Authorization': 'Bearer ' + token },
        success: function() {
            Swal.fire({
            icon: 'success',
            title: 'Thành công!',
            text: 'Loại vàng đã được xóa thành công.',
            confirmButtonText: 'Đồng ý'
            });
            loadGoldTypes();
        },
        error: function(xhr) {
            Swal.fire({
            icon: 'error',
            title: 'Lỗi!',
            text: 'Xóa loại vàng thất bại! ' + (xhr.responseText || ''),
            confirmButtonText: 'Đóng'
            });
        }
        });
    }
    });
}
// ========== GOLD PRICE ==========
function loadGoldTypeOptions(goldTypes) {
    const select = $('#goldPriceTypeFilter');
    const selectModal = $('#goldPriceType');
    select.empty();
    selectModal.empty();
    select.append('<option value="">Tất cả loại vàng</option>'); // Thêm option tất cả
    goldTypes.forEach(gt => {
    select.append(`<option value="${gt.id}">${gt.name}</option>`);
    selectModal.append(`<option value="${gt.id}">${gt.name}</option>`);
    });
    loadGoldPrices('', goldTypes); // Mặc định show tất cả
}
$('#goldPriceTypeFilter').change(function() {
    const goldTypeId = $(this).val();
    loadGoldPrices(goldTypeId);
});
function loadGoldPrices(goldTypeId, goldTypesCache) {
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    
    $.ajax({
    url: 'https://localhost:5000/api/GoldPrice',
    type: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(data) {
        const tbody = $('#goldprice-table tbody');
        tbody.empty();
        const goldTypes = goldTypesCache || $('#goldPriceTypeFilter option').map(function(){return {id:$(this).val(),name:$(this).text()};}).get();
        const goldTypeMap = {};
        goldTypes.forEach(gt => goldTypeMap[gt.id] = gt.name);
        data
        .filter(p => !goldTypeId || p.goldTypeId == goldTypeId) // Nếu goldTypeId rỗng thì show tất cả
        .forEach(p => {
            tbody.append(`
            <tr>
                <td>${goldTypeMap[p.goldTypeId] || ''}</td>
                <td>${p.buyPrice}</td>
                <td>${p.sellPrice}</td>
                <td>${p.recordedAt ? p.recordedAt.replace('T', ' ').substring(0, 16) : ''}</td>
                <td>${p.isActive ? 'Hoạt động' : 'Ngừng'}</td>
                <td>${p.createdDate ? p.createdDate.split('T')[0] : ''}</td>
                <td>
                <button class="btn btn-sm btn-outline-primary me-1" onclick="showEditGoldPriceModal(${p.id})"><i class="fas fa-edit"></i></button>
                <button class="btn btn-sm btn-outline-danger" onclick="deleteGoldPrice(${p.id})"><i class="fas fa-trash"></i></button>
                </td>
            </tr>
            `);
        });
    },
    error: function(xhr) {
        console.error('Error loading gold prices:', xhr);
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Không thể tải danh sách giá vàng! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
        });
    }
    });
}
function showCreateGoldPriceModal() {
    $('#goldPriceModalTitle').text('Thêm giá vàng');
    $('#goldPriceForm')[0].reset();
    $('#goldPriceId').val('');
    $('#goldPriceActiveGroup').hide();
    $('#goldPriceModal').modal('show');
}
function showEditGoldPriceModal(id) {
    const token = localStorage.getItem('accessToken');
    if (!token) {
    Swal.fire({
        icon: 'warning',
        title: 'Cảnh báo!',
        text: 'Bạn chưa đăng nhập!',
        confirmButtonText: 'Đồng ý'
    });
    return;
    }
    
    $.ajax({
    url: `https://localhost:5000/api/GoldPrice/${id}`,
    type: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(p) {
        $('#goldPriceModalTitle').text('Sửa giá vàng');
        $('#goldPriceId').val(p.id);
        $('#goldPriceType').val(p.goldTypeId);
        $('#goldPriceBuy').val(p.buyPrice);
        $('#goldPriceSell').val(p.sellPrice);
        $('#goldPriceRecordedAt').val(p.recordedAt ? p.recordedAt.substring(0, 16) : '');
        $('#goldPriceIsActive').val(p.isActive ? 'true' : 'false');
        $('#goldPriceActiveGroup').show();
        $('#goldPriceModal').modal('show');
    },
    error: function(xhr) {
        Swal.fire({
        icon: 'error',
        title: 'Lỗi!',
        text: 'Không thể lấy thông tin giá vàng! ' + (xhr.responseText || ''),
        confirmButtonText: 'Đóng'
        });
    }
    });
}
$('#goldPriceForm').off('submit').on('submit', function(e) {
    e.preventDefault();
    const id = $('#goldPriceId').val();
    const dto = {
    goldTypeId: parseInt($('#goldPriceType').val()),
    buyPrice: parseFloat($('#goldPriceBuy').val()),
    sellPrice: parseFloat($('#goldPriceSell').val()),
    recordedAt: $('#goldPriceRecordedAt').val()
    };
    if (id) {
    dto.id = parseInt(id);
    dto.isActive = $('#goldPriceIsActive').val() === 'true';
    $.ajax({
        url: `https://localhost:5000/api/GoldPrice/${id}`,
        type: 'PUT',
        headers: {
        'Authorization': 'Bearer ' + localStorage.getItem('accessToken'),
        'Content-Type': 'application/json'
        },
        data: JSON.stringify(dto),
        success: function() {
        Swal.fire({
            icon: 'success',
            title: 'Thành công!',
            text: 'Cập nhật giá vàng thành công.',
            confirmButtonText: 'Đồng ý'
        }).then(() => {
            $('#goldPriceModal').modal('hide');
            loadGoldPrices($('#goldPriceTypeFilter').val());
        });
        },
        error: function(xhr) {
        Swal.fire({
            icon: 'error',
            title: 'Lỗi!',
            text: 'Cập nhật giá vàng thất bại! ' + (xhr.responseText || ''),
            confirmButtonText: 'Đóng'
        });
        }
    });
    } else {
    $.ajax({
        url: 'https://localhost:5000/api/GoldPrice',
        type: 'POST',
        headers: {
        'Authorization': 'Bearer ' + localStorage.getItem('accessToken'),
        'Content-Type': 'application/json'
        },
        data: JSON.stringify(dto),
        success: function() {
        Swal.fire({
            icon: 'success',
            title: 'Thành công!',
            text: 'Thêm giá vàng thành công.',
            confirmButtonText: 'Đồng ý'
        }).then(() => {
            $('#goldPriceModal').modal('hide');
            loadGoldPrices($('#goldPriceTypeFilter').val());
        });
        },
        error: function(xhr) {
        Swal.fire({
            icon: 'error',
            title: 'Lỗi!',
            text: 'Tạo giá vàng thất bại! ' + (xhr.responseText || ''),
            confirmButtonText: 'Đóng'
        });
        }
    });
    }
});
function deleteGoldPrice(id) {
    Swal.fire({
    title: 'Xác nhận xóa?',
    text: 'Bạn có chắc chắn muốn xóa giá vàng này?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#d33',
    cancelButtonColor: '#3085d6',
    confirmButtonText: 'Xóa',
    cancelButtonText: 'Hủy'
    }).then((result) => {
    if (result.isConfirmed) {
        const token = localStorage.getItem('accessToken');
        if (!token) {
        Swal.fire({
            icon: 'warning',
            title: 'Cảnh báo!',
            text: 'Bạn chưa đăng nhập!',
            confirmButtonText: 'Đồng ý'
        });
        return;
        }
        
        $.ajax({
        url: `https://localhost:5000/api/GoldPrice/${id}`,
        type: 'DELETE',
        headers: { 'Authorization': 'Bearer ' + token },
        success: function() {
            Swal.fire({
            icon: 'success',
            title: 'Thành công!',
            text: 'Giá vàng đã được xóa thành công.',
            confirmButtonText: 'Đồng ý'
            });
            loadGoldPrices($('#goldPriceTypeFilter').val());
        },
        error: function(xhr) {
            Swal.fire({
            icon: 'error',
            title: 'Lỗi!',
            text: 'Xóa giá vàng thất bại! ' + (xhr.responseText || ''),
            confirmButtonText: 'Đóng'
            });
        }
        });
    }
    });
}
// Logout function
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
        
        // Hiển thị thông báo thành công
        Swal.fire({
        icon: 'success',
        title: 'Đăng xuất thành công!',
        text: 'Bạn đã được đăng xuất khỏi hệ thống.',
        confirmButtonText: 'Đồng ý'
        }).then(() => {
        // Chuyển hướng về trang đăng nhập
        window.location.href = '../Account/login.html';
        });
    }
    });
}

// ACCOUNT PAGINATION
let accounts = [];
let currentAccountPage = 1;
const accountPageSize = 9;
function renderAccounts(accountsData) {
    accounts = accountsData;
    renderAccountsTablePage(1);
}
function renderAccountsTablePage(page) {
    const tbody = document.getElementById('accountsTableBody');
    tbody.innerHTML = '';
    const sorted = [...accounts].sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate));
    const start = (page - 1) * accountPageSize;
    const end = start + accountPageSize;
    const pageData = sorted.slice(start, end);
    pageData.forEach(acc => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
        <td>${acc.id}</td>
        <td>${acc.username}</td>
        <td>${acc.email}</td>
        <td>${acc.fullName}</td>
        <td><span class="badge ${acc.roleName === 'Manager' ? 'bg-primary' : acc.roleName === 'Employee' ? 'bg-info' : 'bg-secondary'}">${acc.roleName}</span></td>
        <td><span class="badge ${acc.isActive ? 'bg-success' : 'bg-warning text-dark'}">${acc.isActive ? 'Active' : 'Inactive'}</span></td>
        <td>${acc.createdDate ? acc.createdDate.split('T')[0] : ''}</td>
        <td></td>
    `;
    tbody.appendChild(tr);
    });
    renderAccountPagination(sorted.length, page);
}
function renderAccountPagination(total, page) {
    const totalPages = Math.ceil(total / accountPageSize);
    let html = `<nav><ul class="pagination justify-content-center">`;
    html += `<li class="page-item${page === 1 ? ' disabled' : ''}"><a class="page-link" href="#" onclick="gotoAccountPage(${page - 1})">Previous</a></li>`;
    for (let i = 1; i <= totalPages; i++) {
    html += `<li class="page-item${i === page ? ' active' : ''}"><a class="page-link" href="#" onclick="gotoAccountPage(${i})">${i}</a></li>`;
    }
    html += `<li class="page-item${page === totalPages ? ' disabled' : ''}"><a class="page-link" href="#" onclick="gotoAccountPage(${page + 1})">Next</a></li>`;
    html += `</ul></nav>`;
    document.getElementById('accountPagination').innerHTML = html;
}
function gotoAccountPage(page) {
    if (page < 1) return;
    const totalPages = Math.ceil(accounts.length / accountPageSize);
    if (page > totalPages) return;
    currentAccountPage = page;
    renderAccountsTablePage(page);
}
// CATEGORY PAGINATION
let categories = [];
let currentCategoryPage = 1;
const categoryPageSize = 9;
function renderCategories(categoriesData) {
    categories = categoriesData;
    renderCategoriesTablePage(1);
}
function renderCategoriesTablePage(page) {
    const tbody = document.getElementById('categoriesTableBody');
    tbody.innerHTML = '';
    const sorted = [...categories].sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate));
    const start = (page - 1) * categoryPageSize;
    const end = start + categoryPageSize;
    const pageData = sorted.slice(start, end);
    pageData.forEach(cat => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
        <td>${cat.id}</td>
        <td>${cat.name}</td>
        <td>${cat.description || ''}</td>
        <td><span class="badge ${cat.isActive ? 'bg-success' : 'bg-warning text-dark'}">${cat.isActive ? 'Đang hoạt động' : 'Ngừng hoạt động'}</span></td>
        <td>${cat.createdDate ? cat.createdDate.split('T')[0] : ''}</td>
        <td>
        <button class="btn btn-sm btn-outline-primary me-1" onclick="editCategory(${cat.id})">
            <i class="fas fa-edit"></i>
        </button>
        <button class="btn btn-sm btn-outline-danger" onclick="deleteCategory(${cat.id})">
            <i class="fas fa-trash"></i>
        </button>
        </td>
    `;
    tbody.appendChild(tr);
    });
    renderCategoryPagination(sorted.length, page);
}
function renderCategoryPagination(total, page) {
    const totalPages = Math.ceil(total / categoryPageSize);
    let html = `<nav><ul class="pagination justify-content-center">`;
    html += `<li class="page-item${page === 1 ? ' disabled' : ''}"><a class="page-link" href="#" onclick="gotoCategoryPage(${page - 1})">Previous</a></li>`;
    for (let i = 1; i <= totalPages; i++) {
    html += `<li class="page-item${i === page ? ' active' : ''}"><a class="page-link" href="#" onclick="gotoCategoryPage(${i})">${i}</a></li>`;
    }
    html += `<li class="page-item${page === totalPages ? ' disabled' : ''}"><a class="page-link" href="#" onclick="gotoCategoryPage(${page + 1})">Next</a></li>`;
    html += `</ul></nav>`;
    document.getElementById('categoryPagination').innerHTML = html;
}
function gotoCategoryPage(page) {
    if (page < 1) return;
    const totalPages = Math.ceil(categories.length / categoryPageSize);
    if (page > totalPages) return;
    currentCategoryPage = page;
    renderCategoriesTablePage(page);
}
// PRODUCT PAGINATION
let products = [];
let currentProductPage = 1;
const productPageSize = 9;
function renderProducts(productsData) {
    products = productsData;
    renderProductsTablePage(1);
}
function renderProductsTablePage(page) {
    const tbody = document.getElementById('productsTableBody');
    tbody.innerHTML = '';
    const sorted = [...products].sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate));
    const start = (page - 1) * productPageSize;
    const end = start + productPageSize;
    const pageData = sorted.slice(start, end);
    pageData.forEach(prod => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
        <td>${prod.id}</td>
        <td>${prod.name}</td>
        <td>${prod.categoryName || ''}</td>
        <td>${prod.description || ''}</td>
        <td><span class="badge ${prod.isActive ? 'bg-success' : 'bg-warning text-dark'}">${prod.isActive ? 'Đang hoạt động' : 'Ngừng hoạt động'}</span></td>
        <td>${prod.createdDate ? prod.createdDate.split('T')[0] : ''}</td>
        <td>
        <button class="btn btn-sm btn-outline-primary me-1" onclick="editProduct(${prod.id})">
            <i class="fas fa-edit"></i>
        </button>
        <button class="btn btn-sm btn-outline-danger" onclick="deleteProduct(${prod.id})">
            <i class="fas fa-trash"></i>
        </button>
        </td>
    `;
    tbody.appendChild(tr);
    });
    renderProductPagination(sorted.length, page);
}
function renderProductPagination(total, page) {
    const totalPages = Math.ceil(total / productPageSize);
    let html = `<nav><ul class="pagination justify-content-center">`;
    html += `<li class="page-item${page === 1 ? ' disabled' : ''}"><a class="page-link" href="#" onclick="gotoProductPage(${page - 1})">Previous</a></li>`;
    for (let i = 1; i <= totalPages; i++) {
    html += `<li class="page-item${i === page ? ' active' : ''}"><a class="page-link" href="#" onclick="gotoProductPage(${i})">${i}</a></li>`;
    }
    html += `<li class="page-item${page === totalPages ? ' disabled' : ''}"><a class="page-link" href="#" onclick="gotoProductPage(${page + 1})">Next</a></li>`;
    html += `</ul></nav>`;
    document.getElementById('productPagination').innerHTML = html;
}
function gotoProductPage(page) {
    if (page < 1) return;
    const totalPages = Math.ceil(products.length / productPageSize);
    if (page > totalPages) return;
    currentProductPage = page;
    renderProductsTablePage(page);
}

function loadDashboardData() {
    const token = localStorage.getItem('accessToken');
    if (!token) return;

    // Lấy thống kê tổng quan
    $.ajax({
    url: 'https://localhost:5000/api/Transaction/statistics',
    method: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(stats) {
        // Tổng doanh số
        $('#dashboard-total-revenue').text(stats.totalRevenue.toLocaleString('vi-VN') + 'đ');
        // Thu nhập tháng
        $('#dashboard-month-income').text(stats.monthIncome.toLocaleString('vi-VN') + 'đ');
        // So sánh tháng trước (nếu có)
        if (stats.monthCompare) {
        $('#dashboard-month-compare').text(stats.monthCompare);
        }
        // Biểu đồ tròn phân tích theo năm
        if (stats.yearAnalysis && stats.yearAnalysis.length > 0) {
        const ctxPie = document.getElementById('dashboard-pie-chart').getContext('2d');
        new Chart(ctxPie, {
            type: 'pie',
            data: {
            labels: stats.yearAnalysis.map(x => x.year),
            datasets: [{
                data: stats.yearAnalysis.map(x => x.amount),
                backgroundColor: ['#1976d2', '#ffd600', '#43a047', '#e53935', '#8e24aa']
            }]
            },
            options: {responsive: true, plugins: {legend: {position: 'bottom'}}}
        });
        }
    }
    });

    // Load biểu đồ doanh số theo ngày
    loadDailyRevenueChart();

    // Lấy giao dịch gần đây
    $.ajax({
    url: 'https://localhost:5000/api/Transaction/recent',
    method: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(transactions) {
        const tbody = $('#dashboard-recent-transactions');
        tbody.empty();
        transactions.forEach(tran => {
        tbody.append(`
            <tr style="cursor:pointer" onclick="window.open('/Pages/Account/transactionDetail.html?id=${tran.id}', '_blank')">
            <td>${tran.transactionDate.split('T')[0]}</td>
            <td>${tran.receiverName}</td>
            <td>${tran.productNames ? tran.productNames.join(', ') : ''}</td>
            <td>${tran.totalAmount.toLocaleString('vi-VN')}đ</td>
            <td><span class="badge ${tran.status === 'COMPLETED' ? 'bg-success' : 'bg-warning text-dark'}">${tran.status === 'COMPLETED' ? 'Hoàn thành' : 'Chờ xử lý'}</span></td>
            </tr>
        `);
        });
    }
    });
}

let dailyRevenueChart = null;

function loadDailyRevenueChart() {
    const token = localStorage.getItem('accessToken');
    if (!token) return;

    const days = $('#daily-revenue-period').val();
    
    $.ajax({
    url: `https://localhost:5000/api/Transaction/daily-revenue?days=${days}`,
    method: 'GET',
    headers: { 'Authorization': 'Bearer ' + token },
    success: function(data) {
        const labels = data.map(item => {
        const date = new Date(item.date);
        return date.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit' });
        });
        const revenues = data.map(item => item.revenue);
        const transactionCounts = data.map(item => item.transactionCount);

        // Destroy existing chart if it exists
        if (dailyRevenueChart) {
        dailyRevenueChart.destroy();
        }

        const ctx = document.getElementById('daily-revenue-chart').getContext('2d');
        dailyRevenueChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
            label: 'Doanh số (VNĐ)',
            data: revenues,
            borderColor: '#1976d2',
            backgroundColor: 'rgba(25, 118, 210, 0.1)',
            borderWidth: 2,
            fill: true,
            tension: 0.4,
            yAxisID: 'y'
            }, {
            label: 'Số giao dịch',
            data: transactionCounts,
            borderColor: '#ff9800',
            backgroundColor: 'rgba(255, 152, 0, 0.1)',
            borderWidth: 2,
            fill: false,
            tension: 0.4,
            yAxisID: 'y1'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            interaction: {
            mode: 'index',
            intersect: false,
            },
            plugins: {
            legend: {
                position: 'top',
            },
            tooltip: {
                callbacks: {
                label: function(context) {
                    if (context.datasetIndex === 0) {
                    return 'Doanh số: ' + context.parsed.y.toLocaleString('vi-VN') + 'đ';
                    } else {
                    return 'Số giao dịch: ' + context.parsed.y;
                    }
                }
                }
            }
            },
            scales: {
            x: {
                display: true,
                title: {
                display: true,
                text: 'Ngày'
                }
            },
            y: {
                type: 'linear',
                display: true,
                position: 'left',
                title: {
                display: true,
                text: 'Doanh số (VNĐ)'
                },
                ticks: {
                callback: function(value) {
                    return value.toLocaleString('vi-VN') + 'đ';
                }
                }
            },
            y1: {
                type: 'linear',
                display: true,
                position: 'right',
                title: {
                display: true,
                text: 'Số giao dịch'
                },
                grid: {
                drawOnChartArea: false,
                },
            }
            }
        }
        });
    },
    error: function(xhr, status, error) {
        console.error('Error loading daily revenue data:', error);
    }
    });
}

// Gọi hàm khi dashboard hiển thị
$(document).ready(function() {
    if ($('#dashboard-section').is(':visible')) {
    loadDashboardData();
    }
    // Nếu chuyển tab về dashboard thì cũng load lại
    $('#sidebar-dashboard').on('click', function() {
    loadDashboardData();
    });
    
    // Event listener cho dropdown thay đổi khoảng thời gian
    $('#daily-revenue-period').on('change', function() {
    loadDailyRevenueChart();
    });
});