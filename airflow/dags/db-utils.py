import psycopg2
from sqlalchemy import create_engine

# Database connection settings
DB_USER = 'airflow'
DB_PASSWORD = 'airflow'
DB_HOST = 'localhost'  # Or the address of your PostgreSQL container
DB_PORT = '5432'
DB_NAME = 'airflow'

# Create a connection engine
engine = create_engine(f'postgresql://{DB_USER}:{DB_PASSWORD}@{DB_HOST}:{DB_PORT}/{DB_NAME}')

# Insert the validated and cleaned dataframe into PostgreSQL
def insert_into_postgres(df, table_name):
    df.to_sql(table_name, engine, if_exists='replace', index=False)
    print(f"Data inserted into {table_name} table.")