\c dwh;

BEGIN;

--setup dim_dates with data from 200o Jan 1th until 2051 
WITH RECURSIVE date_series AS (
    SELECT DATE '2000-01-01' AS full_date
    UNION ALL
    SELECT (full_date + INTERVAL '1 day')::DATE
    FROM date_series
    WHERE full_date < '2050-12-31'
)
INSERT INTO dim_date (date_key, full_date, year, month, day, week_day)
SELECT 
    EXTRACT(YEAR FROM full_date) * 10000 + EXTRACT(MONTH FROM full_date) * 100 + EXTRACT(DAY FROM full_date) AS date_key,
    full_date,
    EXTRACT(YEAR FROM full_date) AS year,
    EXTRACT(MONTH FROM full_date) AS month,
    EXTRACT(DAY FROM full_date) AS day,
    TO_CHAR(full_date, 'Day') AS week_day
FROM date_series;


-- dim_products - ghost record
INSERT INTO dim_products (product_key, stock_code, description, unit_price) 
VALUES (-1, 'Undefined', 'Undefined', 0)
ON CONFLICT DO NOTHING;

-- dim_customers - ghost record
INSERT INTO dim_customers (customer_key, customer_id, country) 
VALUES (-1, 'Undefined', 'Undefined')
ON CONFLICT DO NOTHING;

COMMIT;