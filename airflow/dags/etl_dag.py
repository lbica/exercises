# from airflow import DAG
# from airflow.operators.bash import BashOperator
# from datetime import datetime, timedelta

# default_args = {
#     'owner': 'airflow',
#     'depends_on_past': False,
#     'start_date': datetime(2025, 1, 1),
#     'retries': 1,
#     'retry_delay': timedelta(minutes=5),
# }

# dag = DAG(
#     'etl_pipeline',
#     default_args=default_args,
#     schedule_interval=None,  # Defined per task
# )

# # Run ETL for Customers & Products Daily
# etl_customers = BashOperator(
#     task_id='etl_customers',
#     bash_command='docker exec etl_service python main.py customers',
#     dag=dag,
#     schedule_interval="@daily",
# )

# etl_products = BashOperator(
#     task_id='etl_products',
#     bash_command='docker exec etl_service python main.py products',
#     dag=dag,
#     schedule_interval="@daily",
# )

# # Run ETL for Orders Hourly
# etl_orders = BashOperator(
#     task_id='etl_orders',
#     bash_command='docker exec etl_service python main.py orders',
#     dag=dag,
#     schedule_interval="@hourly",
# )

# etl_customers >> etl_products >> etl_orders
