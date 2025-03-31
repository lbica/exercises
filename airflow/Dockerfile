FROM apache/airflow:2.5.0

# Install PostgreSQL driver
RUN pip install psycopg2-binary

# Set the working directory
WORKDIR /opt/airflow

# Copy DAGs
COPY dags /opt/airflow/dags

# Initialize the database
RUN airflow db init
