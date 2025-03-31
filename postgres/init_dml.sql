-- for dim_dates since 200-01-01 until 2050-12-30

WITH RECURSIVE date_series AS (
    SELECT date('2000-01-01') AS full_date
    UNION ALL
    SELECT date(full_date, '+1 day')
    FROM date_series
    WHERE full_date < '2050-12-31'
)
INSERT INTO dim_date (date_key, full_date, year, month, day, week_day)
SELECT 
    CAST(strftime('%Y%m%d', full_date) AS INTEGER) AS date_key,
    full_date,
    CAST(strftime('%Y', full_date) AS INTEGER) AS year,
    CAST(strftime('%m', full_date) AS INTEGER) AS month,
    CAST(strftime('%d', full_date) AS INTEGER) AS day,
    CASE strftime('%w', full_date)
        WHEN '0' THEN 'Sunday'
        WHEN '1' THEN 'Monday'
        WHEN '2' THEN 'Tuesday'
        WHEN '3' THEN 'Wednesday'
        WHEN '4' THEN 'Thursday'
        WHEN '5' THEN 'Friday'
        WHEN '6' THEN 'Saturday'
    END AS week_day
FROM date_series;

-- dim_products - ghost record
INSERT OR REPLACE INTO dim_products (product_key, stock_code, description, unit_price) 
VALUES (-1, 'Undefined', 'Undefined', 0);

-- dim_customers - ghost record
INSERT OR REPLACE INTO dim_customers (customer_key, customer_id, country) 
VALUES (-1, 'Undefined', 'Undefined');

-- dim_countries - - ghost record

INSERT OR REPLACE INTO dim_countries (country_key, country_name, region) 
VALUES (-1, 'Undefined', 'Undefined');


INSERT INTO dim_countries (country_name, region) VALUES ('United Kingdom', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Iceland', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Finland', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Italy', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Norway', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Bahrain', 'Middle East');
INSERT INTO dim_countries (country_name, region) VALUES ('Spain', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Portugal', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Switzerland', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Austria', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Cyprus', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Belgium', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Unspecified', 'Unspecified');
INSERT INTO dim_countries (country_name, region) VALUES ('Denmark', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Australia', 'Oceania');
INSERT INTO dim_countries (country_name, region) VALUES ('France', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Germany', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('RSA', 'Africa');
INSERT INTO dim_countries (country_name, region) VALUES ('Greece', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Sweden', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Israel', 'Middle East');
INSERT INTO dim_countries (country_name, region) VALUES ('USA', 'North America');
INSERT INTO dim_countries (country_name, region) VALUES ('Saudi Arabia', 'Middle East');
INSERT INTO dim_countries (country_name, region) VALUES ('Poland', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('United Arab Emirates', 'Middle East');
INSERT INTO dim_countries (country_name, region) VALUES ('Singapore', 'Asia');
INSERT INTO dim_countries (country_name, region) VALUES ('Japan', 'Asia');
INSERT INTO dim_countries (country_name, region) VALUES ('Netherlands', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Lebanon', 'Middle East');
INSERT INTO dim_countries (country_name, region) VALUES ('Brazil', 'South America');
INSERT INTO dim_countries (country_name, region) VALUES ('Czech Republic', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('EIRE', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Channel Islands', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('European Community', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Lithuania', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Canada', 'North America');
INSERT INTO dim_countries (country_name, region) VALUES ('Malta', 'Europe');
INSERT INTO dim_countries (country_name, region) VALUES ('Hong Kong', 'Asia');