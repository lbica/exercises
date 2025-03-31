-- init.sql
CREATE DATABASE dwh;


CREATE TABLE "dim_products" (
	"product_key" INTEGER NOT NULL UNIQUE AUTOINCREMENT,
	-- Business key
	"stock_code" VARCHAR(30) NOT NULL UNIQUE,
	"description" VARCHAR(255) NOT NULL,
	"unit_price" NUMERIC NOT NULL,
	PRIMARY KEY("product_key")
);

-- COMMENT ON COLUMN dim_products.stock_code IS 'Business key';

CREATE TABLE "dim_customers" (
	"customer_key" INTEGER NOT NULL UNIQUE AUTOINCREMENT,
	-- Business key
	"customer_id" VARCHAR(30) NOT NULL UNIQUE,
	"country" VARCHAR(255),
	PRIMARY KEY("customer_key")
);

-- COMMENT ON COLUMN dim_customers.customer_id IS 'Business key';

CREATE TABLE "dim_date" (
	-- Format: YYYYMMDD (e.g., 20250330)
	"date_key" INTEGER NOT NULL UNIQUE,
	-- Business key
	"full_date" DATE NOT NULL UNIQUE,
	"year" INTEGER NOT NULL,
	"month" INTEGER NOT NULL,
	"day" INTEGER NOT NULL,
	"week_day" VARCHAR(255) NOT NULL,
	PRIMARY KEY("date_key")
);

-- COMMENT ON COLUMN dim_date.date_key IS 'Format: YYYYMMDD (e.g., 20250330)';
-- COMMENT ON COLUMN dim_date.full_date IS 'Business key';

CREATE TABLE "fact_orders" (
	"invoice_no" VARCHAR(255) NOT NULL,
	-- stored as YYYYMMDD
	"invoice_date" INTEGER NOT NULL,
	"product_key" INTEGER NOT NULL,
	"quantity" INTEGER NOT NULL,
	"customer_key" INTEGER NOT NULL,
	PRIMARY KEY("invoice_no", "invoice_date", "product_key", "customer_key"),
	FOREIGN KEY("product_key") REFERENCES "dim_products"("product_key"),
	FOREIGN KEY("customer_key") REFERENCES "dim_customers"("customer_key"),
	FOREIGN KEY("invoice_date") REFERENCES "dim_date"("date_key")
);

-- COMMENT ON COLUMN fact_orders.invoice_date IS 'stored as YYYYMMDD';



