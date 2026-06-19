# API test cases

Dung file nay de test tren Swagger tai:

```text
http://localhost:5000/swagger
```

## Category

### Lay danh sach category

```http
GET /api/categories
```

Ket qua mong doi:

- `200 OK`

### Tao category

```http
POST /api/categories
Content-Type: application/json

{
  "name": "Monitor",
  "description": "Man hinh may tinh"
}
```

Ket qua mong doi:

- `201 Created`

### Cap nhat category

```http
PUT /api/categories/1
Content-Type: application/json

{
  "name": "Laptop",
  "description": "May tinh xach tay"
}
```

Ket qua mong doi:

- `204 No Content`

### Xoa category

```http
DELETE /api/categories/3
```

Ket qua mong doi:

- `204 No Content` neu category khong co product lien quan
- Co the loi neu category dang co product do foreign key

## Product

### Lay danh sach product

```http
GET /api/products
```

Ket qua mong doi:

- `200 OK`

### Tim theo keyword

```http
GET /api/products?keyword=logitech
```

### Loc theo category

```http
GET /api/products?categoryId=2
```

### Loc theo khoang gia

```http
GET /api/products?minPrice=300000&maxPrice=1000000
```

### Phan trang

```http
GET /api/products?page=1&pageSize=3
```

### Tao product

```http
POST /api/products
Content-Type: application/json

{
  "name": "Samsung Monitor 24",
  "price": 2500000,
  "stockQuantity": 20,
  "categoryId": 1
}
```

Ket qua mong doi:

- `201 Created`

### Cap nhat product

```http
PUT /api/products/1
Content-Type: application/json

{
  "name": "Dell Inspiron 15 2026",
  "price": 15900000,
  "stockQuantity": 9,
  "categoryId": 1
}
```

Ket qua mong doi:

- `204 No Content`

### Xoa product

```http
DELETE /api/products/1
```

Ket qua mong doi:

- `204 No Content`

