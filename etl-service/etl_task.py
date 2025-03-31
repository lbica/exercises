import os
import sys
from datetime import datetime
from pathlib import Path

import pandas as pd
from sqlalchemy import create_engine

# Database Connection
DB_USER = os.getenv("DB_USER", "airflow")
DB_PASSWORD = os.getenv("DB_PASSWORD", "airflow")
DB_HOST = os.getenv("DB_HOST", "localhost")
DB_NAME = os.getenv("DB_NAME", "airflow")

engine = create_engine(f"postgresql://{DB_USER}:{DB_PASSWORD}@{DB_HOST}:5432/{DB_NAME}")

# root paths
ROOT_PATH = os.path.dirname(os.path.abspath(__file__))  # Use current working directory

# data path
DATA_PATH = os.path.join(ROOT_PATH,"data")


# Log file name prefix
LOG_PREFIX_NAME = datetime.now().strftime('%Y%m%d_%H%M%S')

# Errors types for validation rules
ERROR_EMPTY_COLMN = "empty_column"
ERROR_INVALID_DATATYPE = "invalid_datatype"
ERROR_DUPLICATES = "duplicates"


# file names
TEST_FILE = "test.csv"
TEST_FILE_PATH = os.path.join(DATA_PATH, TEST_FILE)
TEST_ERRORS_FILE = TEST_FILE.join(".bad")

CUSTOMERS_FILE = "customers.csv"
CUSTOMERS_FILE_PATH = os.path.join(DATA_PATH, CUSTOMERS_FILE)
CUSTOMERS_ERROR_FILE = CUSTOMERS_FILE.join(".bad")

PRODUCTS_FILE = "products.csv"
PRODUCTS_FILE_PATH = os.path.join(DATA_PATH, PRODUCTS_FILE)
PRODUCTS_ERROR_FILE = PRODUCTS_FILE.join(".bad")

ORDERS_FILE = "orders.csv"
ORDERS_FILE_PATH = os.path.join(DATA_PATH, ORDERS_FILE)
ORDERS_ERROR_FILE = ORDERS_FILE.join(".bad")

#### method description #####
# description: this method help to identify and logs the empty values for a column name
# params: df, col_name where:
#   df: dataframe name with full data
#   col_name: column name to check
# return: a tuple with errors dataframe and cleaned dataframe
#############################
def empty_column_rule(p_df, p_col_name):
    # data frame that cosists of errors for this rule
    l_df_errors = p_df[p_df[p_col_name].isna()]

    # eliminates the rows with empty values for col_name parameter
    l_df_cleansed = p_df.dropna(subset=[p_col_name])


    # return the cleansed data
    return l_df_cleansed, l_df_errors



#### method description #####
# description: this method help to identify and logs the empty values for a column name
# params: df, col_name where:
#   df: dataframe name with full data
#   col_name: column name to check
# return: a tuple with errors dataframe and cleaned dataframe
#############################
def negative_value_rule(p_df, *columns):
    # Identify rows with negative values
    l_df_errors = p_df[(p_df[columns] < 0).any(axis=1)]

    # Create a cleansed DataFrame without negative values
    l_df_cleansed = p_df[(p_df[columns] >= 0).all(axis=1)]

    # return the cleaned data
    return l_df_cleansed, l_df_errors

#############################
# description: this method help to identify and logs the invalid datatype values for a column name
# params: p_df, p_col_name where:
#   p_df: dataframe name with full data
#   p_col_name: column name to check
# return: a tuple with errors dataframe and cleaned dataframe
#############################
def invalid_datatype_rule(p_df, p_col_name, p_data_type = int):

    # Convert the column to numeric, forcing errors to NaN
    p_df[p_col_name] = pd.to_numeric(p_df[p_col_name], errors="coerce")
    # data frame that cosists of errors for this rule

    # Identify invalid (non-numeric) rows (where NaN was created)
    l_df_errors = p_df[p_df[p_col_name].isna()]

    # eliminates the rows with a diffent datatype for col_name
    l_df_cleansed =p_df.dropna(subset=[p_col_name])

    return l_df_cleansed, l_df_errors

#############################
# description: this method help to identify and logs the duplicates values for a column name
# params: p_df, p_col_name where:
#   p_df: dataframe name with full data
#   p_col_name: column name to check
# return: a tuple with errors dataframe and cleaned dataframe
#############################
def duplicates_rule(p_df, *p_columns):

    # check if columns exists in DataFrame
    missing_cols = [col for col in p_columns if col not in p_df.columns]
    if missing_cols:
        raise ValueError(f"The following columns {missing_cols} doesn't exists in file.")

    # data frame that cosists of errors for this rule
    l_df_errors = p_df[p_df.duplicated(subset=p_columns, keep=False)]

    # drop the duplicates rows but keep only the first one
    l_df_cleansed = p_df.drop_duplicates(subset=p_columns)


    return l_df_cleansed, l_df_errors


#############################
# description: this method will apply validation rules for customers
# params: p_df where:
#   p_df: dataframe name with full data
# return: a cleansed dataframe
#############################
def cleansing_customers(p_df):

    # apply empty column value rule, logs the errors rows and return cleansed data
    l_df_cleaned, l_df_errors = empty_column_rule(p_df, "CustomerID")

    # if any errros then save the errors to file 
    if not l_df_errors.empty:
        log_errors(p_df_errors=l_df_errors,p_error_type=ERROR_EMPTY_COLMN ,p_file_name=CUSTOMERS_ERROR_FILE)

    # print(l_df_cleaned)

    # apply invalid data type rule, logs the errors rows and return cleansed data
    l_df_cleaned, l_df_errors = invalid_datatype_rule(l_df_cleaned, "CustomerID")

    # if any errros then save the errors to file 
    if not l_df_errors.empty:
        log_errors(p_df_errors=l_df_errors, p_error_type=ERROR_INVALID_DATATYPE ,p_file_name=CUSTOMERS_ERROR_FILE)

    # print(l_df_cleaned)

    # apply duplicates rule, logs the errors rows and return cleansed data
    l_df_cleaned, l_df_errors = duplicates_rule(l_df_cleaned, "CustomerID")

    # if any errros then save the errors to file 
    if not l_df_errors.empty:
        log_errors(p_df_errors=l_df_errors, p_error_type=ERROR_DUPLICATES ,p_file_name=CUSTOMERS_ERROR_FILE)

    # print(l_df_cleaned)

    return l_df_cleaned


#############################
# description: this method will apply validation rules for products
# params: p_df where:
#   p_df: dataframe name with full data
# return: a cleansed dataframe
#############################
def cleansing_products(p_df):

    # apply empty column value rule, logs the errors rows and return cleansed data
    l_df_cleaned, l_df_errors = empty_column_rule(p_df, "StockCode")

    # if any errros then save the errors to file 
    if not l_df_errors.empty:
        log_errors(p_df_errors=l_df_errors,p_error_type=ERROR_EMPTY_COLMN ,p_file_name=PRODUCTS_ERROR_FILE)


    # apply invalid data type rule, logs the errors rows and return cleansed data
    l_df_cleaned, l_df_errors = invalid_datatype_rule(l_df_cleaned, "StockCode")

    # if any errros then save the errors to file 
    if not l_df_errors.empty:
        log_errors(p_df_errors=l_df_errors, p_error_type=ERROR_INVALID_DATATYPE ,p_file_name=PRODUCTS_ERROR_FILE)


    # apply duplicates rule, logs the errors rows and return cleansed data
    l_df_cleaned, l_df_errors = duplicates_rule(l_df_cleaned, "StockCode")

    # if any errros then save the errors to file 
    if not l_df_errors.empty:
        log_errors(p_df_errors=l_df_errors, p_error_type=ERROR_DUPLICATES ,p_file_name=PRODUCTS_ERROR_FILE)


    return l_df_cleaned


#############################
# description: this method will apply validation rules for orders
# params: p_df where:
#   p_df: dataframe name with full data
# return: a cleansed dataframe
#############################
def cleansing_orders(p_df):

    # apply empty column value rule, logs the errors rows and return cleansed data
    l_df_cleaned, l_df_errors = empty_column_rule(p_df, "CustomerID")

    # if any errros then save the errors to file 
    if not l_df_errors.empty:
        log_errors(p_df_errors=l_df_errors,p_error_type=ERROR_EMPTY_COLMN ,p_file_name=ORDERS_ERROR_FILE)


    # apply invalid data type rule, logs the errors rows and return cleansed data
    l_df_cleaned, l_df_errors = invalid_datatype_rule(l_df_cleaned, "StockCode")

    # if any errros then save the errors to file 
    if not l_df_errors.empty:
        log_errors(p_df_errors=l_df_errors, p_error_type=ERROR_INVALID_DATATYPE ,p_file_name=ORDERS_ERROR_FILE)


    # apply duplicates rule, logs the errors rows and return cleansed data
    l_df_cleaned, l_df_errors = duplicates_rule(l_df_cleaned, "StockCode")

    # if any errros then save the errors to file 
    if not l_df_errors.empty:
        log_errors(p_df_errors=l_df_errors, p_error_type=ERROR_DUPLICATES ,p_file_name=ORDERS_ERROR_FILE)


    return l_df_cleaned


#############################
# description: # write errors to a error file you can write the error in a generic table in database.
# For simplicity I logged to an .csv file
# params: p_df_errors, p_error_type, p_file_name where:
#   p_df_errors: dataframe with errors data
#   p_error_type: e.g. 'empty_column', 'invalid_datatype', 'duplicates'
#   p_file_name: output file name 
# return: 
#############################
def log_errors(p_df_errors, p_error_type, p_file_name):
    

    # concates with errros folder
    l_dir_error_type = os.path.join(DATA_PATH, "errors", p_error_type)

    # create a path object
    path = Path(l_dir_error_type)

    # Create directories if they don’t exist
    path.mkdir(parents=True, exist_ok=True)

    # compute file name with prefix
    l_file_name = os.path.join(l_dir_error_type,f"{LOG_PREFIX_NAME}_{p_file_name}")
    # write to error storage
    p_df_errors.to_csv(l_file_name, mode='w', index=False)


#############################
# description: # load the final cleansed data to the database/csv file.
# params: df_errors, file_name where:
#   df: df_errors name with erros data
#   file_name: file name to write into
# return: 
#############################
def load_data(p_df_cleansed, p_file_name, table_name, p_schema):


    # Enforce schema before saving
    for col, dtype in p_schema.items():
        p_df_cleansed[col] = p_df_cleansed[col].astype(dtype)

    # concates with errros folder
    l_dir_cleansed = os.path.join(DATA_PATH, "cleansed")

    # create a path object
    path = Path(l_dir_cleansed)

    # Create directories if they don’t exist
    path.mkdir(parents=True, exist_ok=True)

    # compute file name with prefix
    l_file_name = os.path.join(l_dir_cleansed,f"{LOG_PREFIX_NAME}_{p_file_name}")
    # write to file
    p_df_cleansed.to_csv(l_file_name, mode='w', index=False)

    # load to database
    insert_into_postgres(p_df_cleansed, table_name=table_name)


#############################
# description: # load the final cleansed data to the database.
# params: df, table_name where:
#   df: dataframe with cleansed data
#   table_name: table name to write into
# return: 
#############################
def insert_into_postgres(df, table_name):
    df.to_sql(table_name, engine, if_exists='replace', index=False)
    print(f"Data inserted into {table_name} table.")



#############################
# description: # processing customer entity
# params: p_file_name where:
#   p_file_name: input file name
# return: 
#############################
def processing_customers(p_file_name):
    # read the input file
    l_df = pd.read_csv(p_file_name)
    l_df_cleansed = cleansing_customers(l_df)

    # define schema due pandas use implicit type when loading from csv
    schema = {
        "CustomerID": "int64",     
        "Country": "string",
    }


    # load_data(l_df_cleansed, CUSTOMERS_FILE, "customers", schema)


#############################
# description: # processing products entity
# params: p_file_name where:
#   p_file_name: input file name
# return: 
#############################
def processing_products(p_file_name):
    # read the input file
    l_df = pd.read_csv(p_file_name)
    l_df_cleansed = cleansing_products(l_df)

    # define schema due pandas use implicit type when loading from csv
    schema = {
        "StockCode": "string",     
        "Description": "string",
        "UnitPrice": "float64"
    }


    load_data(l_df_cleansed, PRODUCTS_FILE, "dim_products", schema)


#############################
# description: # processing orders entity
# params: p_file_name where:
#   p_file_name: input file name
# return: 
#############################
def processing_orders(p_file_name):
    # read the input file
    l_df = pd.read_csv(p_file_name)
    l_df_cleansed = cleansing_orders(l_df)

    # define schema due pandas use implicit type when loading from csv
    schema = {
        "InvoiceNo": "string",     
        "StockCode": "string",
        "Quantity": "int64",
        "InvoiceDate": "date",
        "CustomerID": "string"
    }


    load_data(l_df_cleansed, ORDERS_FILE, "fact_orders", schema)


if __name__ == "__main__":
       

    # check the number of arguments and execute the apprpiate method for passed entity
    if len(sys.argv) > 1:
        if sys.argv[1] == "customers":
            processing_customers(CUSTOMERS_FILE_PATH)
        elif sys.argv[1] == "products":
            pass
            # processing_products(PRODUCTS_ERROR_FILE)
        elif sys.argv[1] == "orders":
            pass
            # processing_orders(ORDERS_FILE_PATH)
    else:
        print("Please provide a valid argument: customers, products or orders,"
        "example: main_task customers")
