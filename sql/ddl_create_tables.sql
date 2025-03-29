CREATE TABLE customers (
    customer_id INT,
    country VARCHAR(255)
);

CREATE TABLE products (
    stock_code VARCHAR(50),
    product_description VARCHAR(255),
    unit_price DECIMAL(10,2)
);


CREATE TABLE orders (
    invoice_no INT,
    stock_code VARCHAR(50),
    quantity INT,
    invoice_date DATE,
    customer_id INT
    /*
    FOREIGN KEY (customer_id) REFERENCES customers(customer_id),
    FOREIGN KEY (product_id) REFERENCES products(product_id)
    */
);
