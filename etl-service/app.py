import os
import traceback

from flask import Flask, request, jsonify
import pandas as pd
from sqlalchemy import create_engine

app = Flask(__name__)
os.environ["FLASK_ENV"] = "uat"

POSTGRES_CONN = "postgresql://airflow:airflow@postgres:5432/dwh"
# ROOT_DATA = "/app/data/"

# root path
ROOT_PATH = os.path.dirname(os.path.abspath(__file__))  # Use current working directory

# add slash (os dependent)
ROOT_PATH = os.path.join(ROOT_PATH, '')

# data path
DATA_PATH = os.path.join(ROOT_PATH, "data", '')


# Define schemas and mappings for different tables
SCHEMA_MAP = {
    "dim_customers": {
        "rename_map": {"CustomerID": "customer_id", "Country": "country"},
        "schema": {"customer_id": "int64", "country": "string"},
        # validation rules
        "required_columns": ["CustomerID", "Country"],
        "duplicates_columns": ["CustomerID"],
        "negative_columns": [],
        "invalid_date_columns": [],
    },
    "dim_products": {
        "rename_map": {"StockCode": "stock_code", "Description": "description", "UnitPrice": "unit_price"},
        "schema": {"stock_code": "string", "description": "string", "unit_price": "float"},
        # validation rules
        "required_columns": ["StockCode", "Description", "UnitPrice"],
        "duplicates_columns": ["StockCode"],
        "negative_columns": [],
        "invalid_date_columns": [],
    },
    "fact_orders": {
        "rename_map": {"InvoiceNo": "invoice_no", "CustomerID": "customer_id", "StockCode": "stock_code", "Quantity": "quantity"},
        "schema": {"invoice_no": "string", "customer_key": "int64", "product_key": "int64", "invoice_date": "int64", "quantity": "int64"},
        # validation rules
        "required_columns": ["InvoiceNo", "CustomerID", "StockCode", "InvoiceDate", "Quantity"],
        "duplicates_columns": ["InvoiceNo", "CustomerID", "StockCode", "InvoiceDate", "Quantity"],
        "negative_columns": ["Quantity"],
        "invalid_date_columns": ["InvoiceDate"],
    },
}

# Generic cleaning function
def clean_and_transform_data(df, table_name):
    """Cleans data, renames columns, and enforces schema based on table_name."""
    if table_name not in SCHEMA_MAP:
        raise ValueError(f"Schema not defined for table: {table_name}")

    config = SCHEMA_MAP[table_name]
    rename_map = config["rename_map"]
    schema = config["schema"]
    required_columns = config["required_columns"]
    duplicates_columns = config["duplicates_columns"]
    # if config[negative_columns]:
    #     negative_columns = config[negative_columns]

    # Drop duplicates and check required columns
    df.drop_duplicates(subset=duplicates_columns, inplace=True)
    df.dropna(subset=required_columns, inplace=True)

    # check negative values
    # valid_rows = valid_rows[(valid_rows[negative_columns] >= 0).all(axis=1)]

    # Rename columns
    df.rename(columns=rename_map, inplace=True)

    # Enforce schema (convert types)
    for col, dtype in schema.items():
        df[col] = df[col].astype(dtype)

    return df

# Load data into PostgreSQL
def load_to_postgres(table_name, df):
    engine = create_engine(POSTGRES_CONN)

    df.to_sql(table_name, engine, if_exists="append", index=False)


# Load reference data for validation
def load_reference_data():
    engine = create_engine(POSTGRES_CONN)
    customers = pd.read_sql("SELECT customer_id FROM dim_customers", engine)
    products = pd.read_sql("SELECT stock_code FROM dim_products", engine)
    return customers['customer_id'].tolist(), products['stock_code'].tolist()

def validate_orders(df):

# read the configuration map


    """Checks if customer IDs and product codes exist in reference data.
       Returns valid rows and invalid rows separately."""
    
    valid_customers, valid_products = load_reference_data()

    # Identify invalid rows
    invalid_customers = df[~df['CustomerID'].isin(valid_customers)]
    invalid_products = df[~df['StockCode'].isin(valid_products)]
    
    # Merge invalid rows for reporting
    # invalid_rows = pd.concat([invalid_customers, invalid_products]).drop_duplicates(subset=duplicates_columns, inplace=True)

    # Keep only valid rows
    valid_rows = df[df['CustomerID'].isin(valid_customers) & df['StockCode'].isin(valid_products)]


    return valid_rows

# API to run daily ETL (Customers & Products)
@app.route("/run_daily", methods=["POST"])
def run_daily():
    try:
        for file, table in [("customers.csv", "dim_customers"), ("products.csv", "dim_products")]:
            df = pd.read_csv(f"{DATA_PATH}{file}")
            # please note that due the time preasure I didn't log the bad data
            df = clean_and_transform_data(df, table)
            load_to_postgres(table, df)
            # df.to_csv(f"{DATA_PATH}cleansed{file}", mode='w', index=False)

        return jsonify({"status": "success", "message": "Daily ETL completed"}), 200
    except Exception as e:

        error = str(e)
        if os.getenv("FLASK_ENV") == "Development":
            error = traceback.format_exc()
            
        return jsonify({"status": "error", "message": error}), 500

# API to run hourly ETL (Orders)
@app.route("/run_hourly", methods=["POST"])
def run_hourly():
    try:
        orders = pd.read_csv(f"{DATA_PATH}orders.csv")
        orders = clean_and_transform_data(orders, "fact_orders")

        # Validate orders against reference data
        valid_orders, invalid_orders = validate_orders(orders)
        load_to_postgres("fact_orders", valid_orders)

        # Save invalid orders separately
        if not invalid_orders.empty:
            invalid_orders.to_csv(f"{DATA_PATH}invalid_orders.csv", index=False)

        return jsonify({
            "status": "success",
            "message": "Hourly ETL completed",
            "invalid_records": len(invalid_orders)
        }), 200
    except Exception as e:
        return jsonify({"status": "error", "message": str(e)}), 500


@app.route("/")
def home():
    return "ETL Service is running!"


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)
