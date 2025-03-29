import pandas as pd


from airflow import DAG
from airflow.operators.python import PythonOperator
from datetime import datetime, timedelta

# Define the DAG
dag = DAG('data_ingestion_dag',
          description='Data Ingestion for Products, Customers, and Orders',
          schedule_interval='@daily',  # Run daily for customers and products, hourly for orders
          start_date=datetime(2025, 1, 1),
          catchup=False)

# Python functions to process the data
def process_products():
    df = read_data('products.csv')
    df_clean = validate_products(df)
    insert_into_postgres(df_clean, 'dim_products')

def process_customers():
    df = read_data('customers.csv')
    df_clean = validate_customers(df)
    insert_into_postgres(df_clean, 'dim_customers')

def process_orders():
    df = read_data('orders.csv')
    products_df = read_data('products.csv')  # Load the products data for validation
    customers_df = read_data('customers.csv')  # Load the customers data for validation
    df_clean = validate_orders(df, products_df, customers_df)
    insert_into_postgres(df_clean, 'fact_orders')

# Define the Airflow tasks
process_products_task = PythonOperator(
    task_id='process_products',
    python_callable=process_products,
    dag=dag)

process_customers_task = PythonOperator(
    task_id='process_customers',
    python_callable=process_customers,
    dag=dag)

process_orders_task = PythonOperator(
    task_id='process_orders',
    python_callable=process_orders,
    dag=dag)

# Task dependencies (execution order)
process_products_task >> process_customers_task >> process_orders_task

#  Example of reading CSV files for products, customers, and orders
def read_data(file_path):
    return pd.read_csv(file_path)


# Data Validation and Cleaning for Products
def validate_products(df):
    # Check for missing Description or negative UnitPrice
    df = df[df['Description'].notna()]  # Remove rows with missing description
    df = df[df['UnitPrice'] > 0]  # Remove rows with non-positive UnitPrice
    
    # Check for duplicate StockCode
    if df['StockCode'].duplicated().any():
        print("Warning: Duplicate StockCodes found.")
        df = df.drop_duplicates(subset='StockCode')
    
    return df

# Data Validation and Cleaning for Customers
def validate_customers(df):
    # Check for missing CustomerID or empty Country
    df = df[df['CustomerID'].notna()]
    df = df[df['Country'].notna()]
    
    # Check for duplicate CustomerID
    if df['CustomerID'].duplicated().any():
        print("Warning: Duplicate CustomerIDs found.")
        df = df.drop_duplicates(subset='CustomerID')
    
    return df

# Data Validation and Cleaning for Orders
def validate_orders(df, products_df, customers_df):
    # Check for missing InvoiceNo, invalid StockCode, negative Quantity, and invalid InvoiceDate
    df = df[df['InvoiceNo'].notna()]
    df = df[df['StockCode'].isin(products_df['StockCode'])]  # StockCode must be in the products dataset
    df = df[df['Quantity'] > 0]  # Quantity must be positive
    df = df[pd.to_datetime(df['InvoiceDate'], errors='coerce').notna()]  # Check for valid dates
    
    # Check if CustomerID exists in customers dataset
    df = df[df['CustomerID'].isin(customers_df['CustomerID'])]
    
    # Handle errors or invalid data
    if df.isnull().sum().any():
        print("Warning: Some rows have invalid data.")
    
    return df

# Example function to handle error logging
def log_errors(df, error_file="errors.log"):
    if df.isnull().sum().any():
        df.to_csv(error_file, mode='a', header=False)
        print(f"Error log updated: {error_file}")