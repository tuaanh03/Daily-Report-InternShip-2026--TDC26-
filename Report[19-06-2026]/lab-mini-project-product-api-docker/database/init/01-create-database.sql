IF DB_ID(N'ProductManagementDb') IS NULL
BEGIN
    CREATE DATABASE ProductManagementDb;
END
GO

USE ProductManagementDb;
GO

IF OBJECT_ID(N'Products', N'U') IS NOT NULL
BEGIN
    DROP TABLE Products;
END
GO

IF OBJECT_ID(N'Categories', N'U') IS NOT NULL
BEGIN
    DROP TABLE Categories;
END
GO

CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL
);
GO

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    StockQuantity INT NOT NULL,
    CategoryId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Products_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Products_Categories
        FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);
GO

INSERT INTO Categories (Name, Description)
VALUES
    (N'Laptop', N'May tinh xach tay'),
    (N'Mouse', N'Chuot may tinh'),
    (N'Keyboard', N'Ban phim may tinh');
GO

INSERT INTO Products (Name, Price, StockQuantity, CategoryId)
VALUES
    (N'Dell Inspiron 15', 15500000, 10, 1),
    (N'HP Pavilion 14', 14200000, 8, 1),
    (N'Logitech M331', 350000, 30, 2),
    (N'Logitech G102', 450000, 25, 2),
    (N'Keychron K2', 2100000, 12, 3),
    (N'Akko 3087', 1600000, 15, 3);
GO

