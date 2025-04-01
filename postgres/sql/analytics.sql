--Top 10 countries with the most number of customers. 
SELECT 
    c.country, 
    COUNT(c.customer_key) AS customer_count
FROM 
    dim_customers c
GROUP BY 
    c.country
ORDER BY 
    customer_count DESC
LIMIT 10;



--Revenue distribution by country
SELECT 
    c.country,
    SUM(fo.quantity * p.unit_price) AS total_revenue
FROM 
    fact_orders fo
INNER JOIN 
    dim_customers c ON fo.customer_key = c.customer_key
INNER JOIN 
    dim_products p ON fo.product_key = p.product_key
GROUP BY 
    c.country
ORDER BY 
    total_revenue DESC;


-- Relationship between average unit price of products and their sales volume
SELECT
	p.stock_code as product,
    p.description AS product_description,
    AVG(p.unit_price) AS average_unit_price,
    SUM(fo.quantity) AS total_sales_volume
FROM 
    fact_orders fo
INNER JOIN 
    dim_products p ON fo.product_key = p.product_key
GROUP BY 
    p.product_key
ORDER BY 
    total_sales_volume DESC;


-- Top 3 products with the maximum unit price drop in the last month. 
WITH cte_lastmonthprice AS (
    SELECT 
        p.product_key,
        p.description,
        p.unit_price AS last_month_price
    FROM 
        dim_products p
    INNER JOIN 
        fact_orders fo ON p.product_key = fo.product_key
    INNER JOIN 
        dim_date d ON fo.invoice_date = d.date_key
    WHERE 
        TO_CHAR(d.date_key, 'MM') = TO_CHAR((CURRENT_DATE - INTERVAL '1 month'), 'MM')
),
cte_currentmonthprice AS (
    SELECT 
        p.product_key,
        p.description,
        p.unit_price AS current_month_price
    FROM 
        dim_products p
    INNER JOIN 
        fact_orders fo ON p.product_key = fo.product_key
    INNER JOIN 
        dim_date d ON fo.invoice_date = d.date_key
    WHERE 
        TO_CHAR(d.date_key, 'MM') = TO_CHAR(CURRENT_DATE, 'MM')
)
SELECT 
    lmp.product_key,
    lmp.description,
    lmp.last_month_price,
    cmp.current_month_price,
    (lmp.last_month_price - cmp.current_month_price) AS price_drop
FROM 
    cte_lastmonthprice lmp
INNER JOIN 
    cte_currentmonthprice cmp ON lmp.product_key = cmp.product_key
ORDER BY 
    price_drop DESC
LIMIT 3;

