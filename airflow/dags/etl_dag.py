from airflow import DAG
from airflow.providers.http.operators.http import SimpleHttpOperator
from airflow.providers.http.sensors.http import HttpSensor
from airflow.utils.dates import days_ago
from datetime import timedelta

# Define the DAG
dag = DAG(
    'etl_service_api_call',
    default_args={
        'owner': 'airflow',
        'retries': 3,
        'retry_delay': timedelta(minutes=5),
    },
    description='Call ETL Service API endpoints',
    schedule_interval=None,  # DAG schedule
    start_date=days_ago(1),
    catchup=False,
)

# Define connection parameters
etl_service_base_url = 'http://bis-coding-exercise-etl-service-1:5000'

# Use HttpSensor to check if the API is up and running
check_api_status = HttpSensor(
    task_id='check_etl_service_status',
    http_conn_id='etl_service_connection',  # 
    endpoint='',
    method='GET',
    poke_interval=10,
    timeout=30,
    retries=3,
    dag=dag,
)

# Call @run_daily endpoint
run_daily_task = SimpleHttpOperator(
    task_id='run_daily_etl',
    http_conn_id='etl_service_connection',
    endpoint='/run_daily',
    method='POST',  
    data={},  
    headers={'Content-Type': 'application/json'},
    response_check=lambda response: response.status_code == 200, 
    schedule_interval='@daily',  # This will run the task daily
    dag=dag,
)

# Call @run_hourly endpoint
run_hourly_task = SimpleHttpOperator(
    task_id='run_hourly_etl',
    http_conn_id='etl_service_connection',
    endpoint='/run_hourly',
    method='POST',  
    data={},  
    headers={'Content-Type': 'application/json'},
    response_check=lambda response: response.status_code == 200,  
    schedule_interval='@hourly',  # This will run the task every hour
    dag=dag,
)

# Set task dependencies
check_api_status >> [run_daily_task, run_hourly_task]
